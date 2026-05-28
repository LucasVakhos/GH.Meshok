using GH.Configs;
using GH.Utils;
using NewsMaker.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace NewsMaker
{
    public class Letter
    {
        private SendCallBack _callBack;
        public int MailId { get; private set; } = -1;
        public string Name
        {
            get
            {
                return Content.mail.to.name;
            }
        }
        public RootElement Content { get; private set; }
        public Letter(int mailId, string idempotencyKey, string name, string mailto, string unsubscribe_url, string subject = null, string body = null, Attachments attachments = null)
        {
            MailId = mailId;
            Content = new RootElement(idempotencyKey, name, mailto, unsubscribe_url, subject, body, attachments);
        }
        public SendCallBack CallBack
        {
            get => _callBack;
            set
            {
                _callBack = value;
            }
        }
        public string MailTo => Content.mail.to.email;
        public string Subject => Content.mail.subject;
        public string BodyText => Content.mail.text;
        public override string ToString()
        {
            string res = "=======================================================\r\n";
            res += RuSender.MakeRequestString(Content);
            res += "=======================================================\r\n";
            return res;
        }
        internal void RemoveFromBuffer()
        {
            if (MailId > -1)
            {
                try
                {
                    MySqlHelper.ExecQuery($"DELETE FROM subscribers_send_buffer WHERE id = {MailId}");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
        internal void DeactivateSubscriber()
        {
            if (MailId > -1)
            {
                try
                {
                    MySqlHelper.ExecQuery($"UPDATE subscribers s SET s.active = 0 WHERE s.id = {MailId}");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
        internal void RemoveSubscriber()
        {
            if (MailId > -1)
            {
                try
                {
                    MySqlHelper.ExecQuery($"DELETE FROM subscribers WHERE id = {MailId}");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
    public class RootElement
    {
        private SendService ss => SendService.Instance;
        public string idempotencyKey { get; private set; }
        public Mail mail { get; private set; }
        public RootElement(string idempotencyKey, string name, string mailto, string unsubscribe_url,
            string subject = null,
            string body = null,
            Attachments attachments = null)
        {
            this.idempotencyKey = idempotencyKey;
            ToFrom to = new ToFrom(mailto, name);
            ToFrom from = new ToFrom("info@bridgenote.com", "Bridgenote.com");
            if (subject == null) subject = "Обновления на Bridgenote для " + name;
            if (body == null)
            {
                body = ss.GetBodyText(name, unsubscribe_url);
                string bodyHtml = ss.GetBodyHtml(name, unsubscribe_url);
                this.mail = new Mail(to, from, subject, bodyHtml, body, attachments, "Открой архив и посмотри новинки");
            }
            else
            {
                this.mail = new Mail(to, from, subject, body, body);
            }
        }
    }
    public class Mail
    {
        private string _subject;
        private string _html;
        private string _text;
        private string _previewTitle;
        public Mail(ToFrom to, ToFrom from, string subject, string html, string text, Attachments attachments = null, string previewTitle = null)
        {
            this.to = to ?? throw new ArgumentNullException(nameof(to));
            this.from = from ?? throw new ArgumentNullException(nameof(from));
            this.subject = subject ?? throw new ArgumentNullException(nameof(subject));
            this.html = html ?? throw new ArgumentNullException(nameof(html));
            this.text = text ?? throw new ArgumentNullException(nameof(text));
            this.previewTitle = previewTitle;
            this.attachments = attachments;
        }
        public ToFrom to { get; set; }
        public ToFrom from { get; set; }
        public string subject { get => _subject.ToUTF8(); set => _subject = value; }
        public string html { get => _html.ToUTF8(); set => _html = value; }
        public string text { get => _text.ToUTF8(); set => _text = value; }
        public string previewTitle { get => _previewTitle.ToUTF8(); set => _previewTitle = value; }
        public Attachments attachments { get; set; }
    }
    public class ToFrom
    {
        private string _name = string.Empty;
        public ToFrom(string email, string name)
        {
            this.email = email ?? throw new ArgumentNullException(nameof(email));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public string email { get; set; } = string.Empty;
        public string name
        {
            get => _name.ToUTF8();
            set => _name = value;
        }
    }
    [JsonArray(false)]
    public class Attachments : Dictionary<string, string>
    {
    }
}
