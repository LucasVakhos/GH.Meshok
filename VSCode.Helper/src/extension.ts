import * as vscode from 'vscode';

// Интерфейс для хранения информации о Git изменениях
interface GitChange {
	file: string;
	status: string;
	additions: number;
	deletions: number;
	timestamp: Date;
}

class CommentGenerator {
	private lastAutoRun: Date = new Date(0);
	private isAutoRunning: boolean = false;
	private gitChanges: GitChange[] = [];

	/**
	 * Генерирует временную метку в формате [YYYY-MM-DD HH:MM:SS]
	 */
	private generateTimestamp(): string {
		const now = new Date();
		const year = now.getFullYear();
		const month = String(now.getMonth() + 1).padStart(2, '0');
		const day = String(now.getDate()).padStart(2, '0');
		const hours = String(now.getHours()).padStart(2, '0');
		const minutes = String(now.getMinutes()).padStart(2, '0');
		const seconds = String(now.getSeconds()).padStart(2, '0');

		return `[${year}-${month}-${day} ${hours}:${minutes}:${seconds}]`;
	}

	/**
	 * Получает информацию о Git изменениях из выделенного текста
	 */
	private parseGitChanges(text: string): GitChange[] {
		const changes: GitChange[] = [];
		const lines = text.split('\n');

		for (const line of lines) {
			// Парсим строки типа: "M file.ts", "A new_file.cs", etc.
			const match = line.match(/^([MAD])\s+(.+)$/);
			if (match) {
				const status = match[1];
				const file = match[2].trim();
				changes.push({
					file,
					status: this.mapGitStatus(status),
					additions: 0,
					deletions: 0,
					timestamp: new Date()
				});
			}
		}

		return changes;
	}

	/**
	 * Преобразует код Git статуса в понятный вид
	 */
	private mapGitStatus(code: string): string {
		const statusMap: { [key: string]: string } = {
			'M': 'Modified',
			'A': 'Added',
			'D': 'Deleted',
			'R': 'Renamed',
			'C': 'Copied',
			'U': 'Updated'
		};
		return statusMap[code] || 'Unknown';
	}

	/**
	 * Создает комментарий с информацией о Git изменениях
	 */
	private createCommentFromGitChanges(changes: GitChange[]): string {
		const timestamp = this.generateTimestamp();
		let comment = `// ${timestamp}\n`;
		comment += `// Git Changes:\n`;

		for (const change of changes) {
			comment += `// ${change.status}: ${change.file}\n`;
		}

		return comment;
	}

	/**
	 * Создает простой комментарий с временной меткой
	 */
	private createSimpleComment(): string {
		const timestamp = this.generateTimestamp();
		return `// ${timestamp}`;
	}

	/**
	 * Вставляет комментарий в активный редактор
	 */
	public async insertComment(commentText: string): Promise<void> {
		const editor = vscode.window.activeTextEditor;
		if (!editor) {
			vscode.window.showWarningMessage('No active editor');
			return;
		}

		await editor.edit((editBuilder) => {
			const position = editor.selection.active;
			editBuilder.insert(position, commentText);
		});
	}

	/**
	 * Выполняет команду создания комментария
	 */
	public async executeCreateComment(): Promise<void> {
		try {
			const editor = vscode.window.activeTextEditor;
			if (!editor) {
				vscode.window.showWarningMessage('No active editor');
				return;
			}

			// Пытаемся получить выделенный текст (может быть Git изменения из клипборда)
			const selectedText = editor.document.getText(editor.selection);

			let comment: string;
			if (selectedText && selectedText.includes('M ') || selectedText.includes('A ') || selectedText.includes('D ')) {
				// Это похоже на Git изменения
				const changes = this.parseGitChanges(selectedText);
				comment = this.createCommentFromGitChanges(changes);
			} else {
				// Просто создаём комментарий с временной меткой
				comment = this.createSimpleComment();
			}

			await this.insertComment(comment + '\n');
			vscode.window.showInformationMessage('Comment inserted!');
		} catch (error) {
			vscode.window.showErrorMessage(`Error: ${error instanceof Error ? error.message : String(error)}`);
		}
	}

	/**
	 * Выполняет команду создания комментария из Git изменений
	 */
	public async executeCreateCommentFromGit(): Promise<void> {
		try {
			const editor = vscode.window.activeTextEditor;
			if (!editor) {
				vscode.window.showWarningMessage('No active editor');
				return;
			}

			// Пытаемся получить содержимое из Git панели или из клипборда
			const clipboardText = await vscode.env.clipboard.readText();

			// Проверяем, является ли это Git информацией
			if (clipboardText && (clipboardText.includes('M ') || clipboardText.includes('A '))) {
				const changes = this.parseGitChanges(clipboardText);
				const comment = this.createCommentFromGitChanges(changes);
				await this.insertComment(comment + '\n');
				vscode.window.showInformationMessage('Git comment inserted!');
			} else {
				vscode.window.showWarningMessage('No Git changes found in clipboard');
			}
		} catch (error) {
			vscode.window.showErrorMessage(`Error: ${error instanceof Error ? error.message : String(error)}`);
		}
	}

	/**
	 * Автоматически создаёт комментарий при активации редактора
	 */
	public async onEditorActive(): Promise<void> {
		if (this.isAutoRunning) {
			return;
		}

		const now = new Date();
		if ((now.getTime() - this.lastAutoRun.getTime()) < 2000) {
			return;
		}

		this.isAutoRunning = true;
		this.lastAutoRun = now;

		try {
			const editor = vscode.window.activeTextEditor;
			if (editor && editor.document.fileName.includes('git')) {
				// Если это Git-связанный файл, пытаемся создать комментарий из Git информации
				await this.executeCreateCommentFromGit();
			}
		} catch (error) {
			// Силently ignore errors
		} finally {
			this.isAutoRunning = false;
		}
	}
}

let commentGenerator: CommentGenerator;

export function activate(context: vscode.ExtensionContext) {
	console.log('VSCode Helper extension is now active');

	commentGenerator = new CommentGenerator();

	// Регистрируем команду для создания комментария
	const createCommentDisposable = vscode.commands.registerCommand(
		'vscode-helper.createComment',
		async () => {
			await commentGenerator.executeCreateComment();
		}
	);

	// Регистрируем команду для создания комментария из Git
	const createCommentFromGitDisposable = vscode.commands.registerCommand(
		'vscode-helper.createCommentFromGit',
		async () => {
			await commentGenerator.executeCreateCommentFromGit();
		}
	);

	// Подписываемся на события смены активного редактора
	const changeActiveEditorDisposable = vscode.window.onDidChangeActiveTextEditor(
		async (editor) => {
			if (editor) {
				await commentGenerator.onEditorActive();
			}
		}
	);

	context.subscriptions.push(
		createCommentDisposable,
		createCommentFromGitDisposable,
		changeActiveEditorDisposable
	);
}

export function deactivate() {
	console.log('VSCode Helper extension is deactivated');
}
