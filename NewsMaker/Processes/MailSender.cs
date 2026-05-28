//#define NOT_SEND
using GH.AppContext;
using GH.Configs;
using NewsMaker.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;
namespace NewsMaker
{
    public class MailSender : Prepare<Letter>
    {
        private readonly object _locker = new object();
        private readonly NewsProcessor _processor;
        public MailSender() : base("", false)
        {
        }
        public MailSender(NewsProcessor processor, bool regIt = true) : base("Подготовка списка рассылки", regIt)
        {
            _processor = processor;
        }
        public int Current { get; private set; }
        /*
        public void Add(string mailName, string mailto, string subject, string message)
        {
            lock (_locker)
            {
                AddItem(new Letter(mailName, mailto, subject, message));
            }
        }
        */
        protected override void FillTable()
        {
            //do nothing
        }
        protected override void NotifyInfo()
        {
            //do nothing
        }
        public void Add(int mailId, string mailName, string mailTo, string idempotencyKey,
            string unsubscribe_url = null,
            string subject = null,
            string message = null
            )
        {
            lock (_locker)
            {
                Letter letter = new Letter(mailId, idempotencyKey, mailName, mailTo, unsubscribe_url, subject, message, Attachments);
                AddItem(letter);
            }
        }
        private void smtpSendMail(RuSender rusender, Letter letter)
        {
            ResponseData result = RunContext.AppMainForm.InvokeIfRequired(new Func<ResponseData>(() => rusender.sendRequest(letter)));
            letter.CallBack = new SendCallBack(result);
            Informator.RegObject(letter);
        }
        private Attachments _attachment = null;
        private Attachments Attachments
        {
            get
            {
                if (_attachment == null)
                {
                    _attachment = new Attachments();
                    byte[] contentBytes = sendService.Zip(out string zipName);
                    string base64FileContent = Convert.ToBase64String(contentBytes);
                    _attachment.Add(zipName, base64FileContent);
                }
                //return null;
                return _attachment;
            }
        }
        public SendCallBack SendAllMails()
        {
            Current = 0;
            CfgRuSender cfgRuSender = IniHelper.CoreCfg<CfgRuSender>();
            RuSender rusender = new RuSender(cfgRuSender.ID, cfgRuSender.ApiKey);
            Letter letter;
            while (GetNextLetter(out letter))
            {
                if (!AppContextNM.Executing)
                    break;
                do
                {
                    Application.DoEvents();
                } while (sendService.NeedWait);
#if NOT_SEND
                letter.CallBack = new SendCallBack(null);
                break;
#else
                smtpSendMail(rusender, letter);
#endif
                if (letter.CallBack.Result)
                {
                    RemoveLetter(letter);
                    IncStep();
                }
                else
                if (letter.CallBack.HasError)
                {
                    Info.RegError(letter.CallBack.ErrorMess);
                    if (letter.CallBack.Content.TryGetValue("unprocess_reason", out object unprocess_reason))
                    {
                        switch ((UnprocessReason)unprocess_reason)
                        {
                            case UnprocessReason.ApiKeyNotFound:
                            case UnprocessReason.UserDomainNotFound:
                            case UnprocessReason.TemplateMailUserNotFoundById:
                                AppContextNM.Executing = false;
                                break;
                            case UnprocessReason.MailUuidNotFound:
                                break;
                            case UnprocessReason.ReceiverUnsubscribed:
                                letter.DeactivateSubscriber();
                                RemoveLetter(letter);
                                continue;
                            case UnprocessReason.ReceiverComplained:
                            case UnprocessReason.ReceiverNotExist:
                                letter.RemoveSubscriber();
                                RemoveLetter(letter);
                                continue;
                            case UnprocessReason.ReceiverUnavailable:
                                RemoveLetter(letter);
                                continue;
                            case UnprocessReason.TemplateNotFound:
                            case UnprocessReason.AanotherUnSentReason:
                                AppContextNM.Executing = false;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        AppContextNM.Executing = false;
                }
                if (!AppContextNM.Executing)
                    break;
            }
            if (AppContextNM.Executing)
            {
                Thread.Sleep(500);
                sendService.SetNeedToSend(Count);
            }
            return letter == null ? new SendCallBack(null) : letter.CallBack;
        }
        private void RemoveLetter(Letter letter)
        {
            letter.RemoveFromBuffer();
            lock (_locker)
            {
                if (Count > 0)
                    Remove(letter);
            }
        }
        private bool GetNextLetter(out Letter letter)
        {
            letter = null;
            lock (_locker)
            {
                if (Count > 0)
                    letter = this[0];
                Current++;
            }
            return letter != null;
        }
        public SendCallBack ErrCallBack(Exception e)
        {
            ResponseData result = new ResponseData();
            result.RegError(e);
            return new SendCallBack(result);
        }
        public SendCallBack SendByYandex()
        {
            if (!GetNextLetter(out Letter letter))
                return ErrCallBack(new Exception("Писем нет!"));
            MailMessage mail = new MailMessage();
            try
            {
                mail.From = new MailAddress(cfgPost.BridgeEmail, "BridgeNote.com");
                mail.To.Add(new MailAddress(letter.MailTo, letter.Name));
                mail.Subject = letter.Subject;
                mail.Body = letter.BodyText;
                SmtpClient client = new SmtpClient();
                try
                {
                    //client.Timeout = 5000;
                    client.Host = cfgPost.Smtp;
                    client.Port = cfgPost.Port;
                    client.EnableSsl = cfgPost.UseSSL;
                    client.Credentials = new NetworkCredential(cfgPost.User, cfgPost.PassWrd);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mail);
                }
                catch (SmtpFailedRecipientException e)
                {
                    return ErrCallBack(e);
                }
                catch (SmtpException e)
                {
                    return ErrCallBack(e);
                }
                catch (Exception e)
                {
                    return ErrCallBack(e);
                }
                finally
                {
                    client.Dispose();
                }
            }
            catch (Exception e)
            {
                return ErrCallBack(e);
            }
            return new SendCallBack(null);
        }
    }
}
