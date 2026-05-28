using GH.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static GH.Utils.DlgHelper;
namespace NewsMaker
{
    internal class Sendpulse : ISendpulse
    {
        private readonly string _apiurl = "https://api.sendpulse.com";
        private int _refreshToken;
        private readonly string _secret;
        private string _tokenName;
        private readonly string _userId;
        public Sendpulse(string userId, string secret)
        {
            if (userId == null || secret == null)
                DlgError("Empty ID or SECRET");
            _userId = userId;
            _secret = secret;
            _tokenName = md5(_userId + "::" + _secret);
            if (_tokenName != null)
                if (!getToken())
                    DlgError("Could not connect to api, check your ID and SECRET");
        }
        public Dictionary<string, object> listAddressBooks(int limit, int offset)
        {
            var data = new Dictionary<string, object>();
            if (limit > 0) data.Add("limit", limit);
            if (offset > 0) data.Add("offset", offset);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks", "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> getBookInfo(int id)
        {
            if (id <= 0) return handleError("Empty book id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + id, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> getEmailsFromBook(int id)
        {
            if (id <= 0)
                return handleError("Empty book id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + id + "/emails", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> removeAddressBook(int id)
        {
            if (id <= 0)
                return handleError("Empty book id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + id, "DELETE", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> editAddressBook(int id, string newname)
        {
            if (id <= 0 || newname.Length == 0)
                return handleError("Empty new name or book id");
            var data = new Dictionary<string, object>();
            data.Add("name", newname);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + id, "PUT", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> createAddressBook(string bookName)
        {
            if (bookName.Length == 0) return handleError("Empty book name");
            var data = new Dictionary<string, object>();
            data.Add("bookName", bookName);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> addEmails(int bookId, string emails)
        {
            if (bookId <= 0 || emails.Length == 0) return handleError("Empty book id or emails");
            var data = new Dictionary<string, object>();
            data.Add("emails", emails);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + bookId + "/emails", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> removeEmails(int bookId, string emails)
        {
            if (bookId <= 0 || emails.Length == 0) return handleError("Empty book id or emails");
            var data = new Dictionary<string, object>();
            data.Add("emails", emails);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + bookId + "/emails", "DELETE", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> getEmailInfo(int bookId, string email)
        {
            if (bookId <= 0 || email.Length == 0) return handleError("Empty book id or email");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + bookId + "/emails/" + email, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> campaignCost(int bookId)
        {
            if (bookId <= 0) return handleError("Empty book id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("addressbooks/" + bookId + "/cost", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> listCampaigns(int limit, int offset)
        {
            var data = new Dictionary<string, object>();
            if (limit > 0) data.Add("limit", limit);
            if (offset > 0) data.Add("offset", offset);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("campaigns", "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> getCampaignInfo(int id)
        {
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("campaigns/" + id, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> campaignStatByCountries(int id)
        {
            if (id <= 0) return handleError("Empty campaign id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("campaigns/" + id + "/countries", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> campaignStatByReferrals(int id)
        {
            if (id <= 0) return handleError("Empty campaign id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("campaigns/" + id + "/referrals", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public Dictionary<string, object> createCampaign(string senderName, string senderEmail, string subject,
            string body, int bookId, string name, string send_date = "", string attachments = "")
        {
            if (senderName.Length == 0 || senderEmail.Length == 0 || subject.Length == 0 || body.Length == 0 ||
                bookId <= 0) return handleError("Not all data.");
            var encodedBody = Base64Encode(body);
            var data = new Dictionary<string, object>();
            if (attachments.Length > 0) data.Add("attachments", attachments);
            if (send_date.Length > 0) data.Add("send_date", send_date);
            data.Add("sender_name", senderName);
            data.Add("sender_email", senderEmail);
            data.Add("subject", subject);
            if (encodedBody.Length > 0) data.Add("body", encodedBody);
            data.Add("list_id", bookId);
            if (name.Length > 0) data.Add("name", name);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("campaigns", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Cancel campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, object> cancelCampaign(int id)
        {
            if (id <= 0) return handleError("Empty campaign id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("campaigns/" + id, "DELETE", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of allowed senders
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> listSenders()
        {
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("senders", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Add new sender
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="senderEmail"></param>
        /// <returns></returns>
        public Dictionary<string, object> addSender(string senderName, string senderEmail)
        {
            if (senderName.Length == 0 || senderEmail.Length == 0) return handleError("Empty sender name or email");
            var data = new Dictionary<string, object>();
            data.Add("name", senderName);
            data.Add("email", senderEmail);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("senders", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Remove sender
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> removeSender(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            var data = new Dictionary<string, object>();
            data.Add("email", email);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("senders", "DELETE", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Activate sender using code from mail
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<string, object> activateSender(string email, string code)
        {
            if (email.Length == 0 || code.Length == 0) return handleError("Empty email or activation code");
            var data = new Dictionary<string, object>();
            data.Add("code", code);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("senders/" + email + "/code", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Send mail with activation code on sender email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> getSenderActivationMail(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("senders/" + email + "/code", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get global information about email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> getEmailGlobalInfo(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("emails/" + email, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Remove email address from all books
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> removeEmailFromAllBooks(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("emails/" + email, "DELETE", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get statistic for email by all campaigns
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> emailStatByCampaigns(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("emails/" + email + "/campaigns", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Show emails from blacklist
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> getBlackList()
        {
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("blacklist", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Add email address to blacklist
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public Dictionary<string, object> addToBlackList(string emails)
        {
            if (emails.Length == 0) return handleError("Empty emails");
            var data = new Dictionary<string, object>();
            var encodedemails = Base64Encode(emails);
            data.Add("emails", encodedemails);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("blacklist", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Remove email address from blacklist
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public Dictionary<string, object> removeFromBlackList(string emails)
        {
            if (emails.Length == 0) return handleError("Empty emails");
            var data = new Dictionary<string, object>();
            var encodedemails = Base64Encode(emails);
            data.Add("emails", encodedemails);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("blacklist", "DELETE", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Return user balance
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public Dictionary<string, object> getBalance(string currency)
        {
            var url = "balance";
            if (currency.Length > 0)
            {
                currency = currency.ToUpper();
                url = url + "/" + currency;
            }
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest(url, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Send mail using SMTP
        /// </summary>
        /// <param name="emaildata"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpSendMail(Dictionary<string, object> emaildata)
        {
            if (emaildata.Count == 0)
                return handleError("Empty email data");
            var html = emaildata["html"].ToString();
            emaildata.Remove("html");
            emaildata.Add("html", Base64Encode(html));
            var data = new Dictionary<string, object>();
            var serialized = JsonConvert.SerializeObject(emaildata);
            data.Add("email", serialized);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/emails", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of emails that was sent by SMTP
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpListEmails(int limit, int offset, string fromDate, string toDate,
            string sender, string recipient)
        {
            var data = new Dictionary<string, object>();
            data.Add("limit", limit);
            data.Add("offset", offset);
            if (fromDate.Length > 0)
                data.Add("fromDate", fromDate);
            if (toDate.Length > 0)
                data.Add("toDate", toDate);
            if (sender.Length > 0)
                data.Add("sender", sender);
            if (recipient.Length > 0)
                data.Add("recipient", recipient);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/emails", "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get information about email by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpGetEmailInfoById(string id)
        {
            if (id.Length == 0) return handleError("Empty id");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/emails/" + id, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Unsubscribe emails using SMTP
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpUnsubscribeEmails(string emails)
        {
            if (emails.Length == 0) return handleError("Empty emails");
            var data = new Dictionary<string, object>();
            data.Add("emails", emails);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/smtp/unsubscribe", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Remove emails from unsubscribe list using SMTP
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpRemoveFromUnsubscribe(string emails)
        {
            if (emails.Length == 0) return handleError("Empty emails");
            var data = new Dictionary<string, object>();
            data.Add("emails", emails);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/smtp/unsubscribe", "DELETE", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of allowed IPs using SMTP
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> smtpListIP()
        {
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/ips", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of allowed domains using SMTP
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> smtpListAllowedDomains()
        {
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/domains", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Add domain using SMTP
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpAddDomain(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            var data = new Dictionary<string, object>();
            data.Add("email", email);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/domains", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Send confirm mail to verify new domain
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Dictionary<string, object> smtpVerifyDomain(string email)
        {
            if (email.Length == 0) return handleError("Empty email");
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("smtp/domains/" + email, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of push campaigns
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushListCampaigns(int limit, int offset)
        {
            var data = new Dictionary<string, object>();
            if (limit > 0) data.Add("limit", limit);
            if (offset > 0) data.Add("offset", offset);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("push/tasks", "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get push campaigns info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushCampaignInfo(int id)
        {
            if (id > 0)
            {
                Dictionary<string, object> result = null;
                try
                {
                    result = sendRequest("push/tasks/" + id, "GET", null);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("No such push campaign");
        }
        /// <summary>
        ///     Get amount of websites
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> pushCountWebsites()
        {
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("push/websites/total", "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of websites
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushListWebsites(int limit, int offset)
        {
            var data = new Dictionary<string, object>();
            if (limit > 0) data.Add("limit", limit);
            if (offset > 0) data.Add("offset", offset);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("push/websites", "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of all variables for website
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushListWebsiteVariables(int id)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (id > 0)
            {
                url = "push/websites/" + id + "/variables";
                try
                {
                    result = sendRequest(url, "GET", null);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get list of subscriptions for the website
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushListWebsiteSubscriptions(int id, int limit, int offset)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (id > 0)
            {
                var data = new Dictionary<string, object>();
                if (limit > 0) data.Add("limit", limit);
                if (offset > 0) data.Add("offset", offset);
                url = "push/websites/" + id + "/subscriptions";
                try
                {
                    result = sendRequest(url, "GET", data);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Get amount of subscriptions for the site
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushCountWebsiteSubscriptions(int id)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (id > 0)
            {
                url = "push/websites/" + id + "/subscriptions/total";
                try
                {
                    result = sendRequest(url, "GET", null);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Set state for subscription
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Dictionary<string, object> pushSetSubscriptionState(int id, int state)
        {
            if (id > 0)
            {
                var data = new Dictionary<string, object>();
                data.Add("id", id);
                data.Add("state", state);
                Dictionary<string, object> result = null;
                try
                {
                    result = sendRequest("push/subscriptions/state", "POST", data);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Create new push campaign
        /// </summary>
        /// <param name="taskinfo"></param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        public Dictionary<string, object> createPushTask(Dictionary<string, object> taskinfo,
            Dictionary<string, object> additionalParams)
        {
            var data = taskinfo;
            if (!data.ContainsKey("ttl")) data.Add("ttl", 0);
            if (!data.ContainsKey("title") || !data.ContainsKey("website_id") || !data.ContainsKey("body"))
                return handleError("Not all data");
            if (additionalParams != null && additionalParams.Count > 0)
                foreach (var item in additionalParams)
                    data.Add(item.Key, item.Value);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("push/tasks", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Add phones to address book.
        /// </summary>
        /// <returns>The phones.</returns>
        /// <param name="bookId">Book identifier.</param>
        /// <param name="phones">Phones.</param>
        public Dictionary<string, object> addPhones(int bookId, string phones)
        {
            if (bookId <= 0 || phones.Length == 0) return handleError("Empty book id or phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            data.Add("addressBookId", bookId);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/numbers", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Remove phones from address book.
        /// </summary>
        /// <returns>The phones.</returns>
        /// <param name="bookId">Book identifier.</param>
        /// <param name="phones">Phones.</param>
        public Dictionary<string, object> removePhones(int bookId, string phones)
        {
            if (bookId <= 0 || phones.Length == 0) return handleError("Empty book id or phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            data.Add("addressBookId", bookId);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/numbers", "DELETE", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Update phones.
        /// </summary>
        /// <returns>The phones.</returns>
        /// <param name="bookId">Book identifier.</param>
        /// <param name="phones">Phones.</param>
        /// <param name="variables">Variables.</param>
        public Dictionary<string, object> updatePhones(int bookId, string phones, string variables)
        {
            if (bookId <= 0 || phones.Length == 0) return handleError("Empty book id or phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            data.Add("variables", variables);
            data.Add("addressBookId", bookId);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/numbers", "PUT", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get the phone number info.
        /// </summary>
        /// <returns>The phone info.</returns>
        /// <param name="bookId">Book identifier.</param>
        /// <param name="phoneNumber">Phone number.</param>
        public Dictionary<string, object> getPhoneInfo(int bookId, string phoneNumber)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (bookId > 0)
            {
                url = "/sms/numbers/info/" + bookId + "/" + phoneNumber;
                try
                {
                    result = sendRequest(url, "GET", null);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Add phones to black list.
        /// </summary>
        /// <returns>The phone to black list.</returns>
        /// <param name="phones">Phones.</param>
        /// <param name="description">Description.</param>
        public Dictionary<string, object> addPhonesToBlackList(string phones, string description)
        {
            if (phones.Length == 0) return handleError("Empty phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            if (description != null) data.Add("description", description);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/black_list", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Remove phones from black list.
        /// </summary>
        /// <returns>The phones from black list.</returns>
        /// <param name="phones">Phones.</param>
        public Dictionary<string, object> removePhonesFromBlackList(string phones)
        {
            if (phones.Length == 0) return handleError("Empty phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/black_list", "DELETE", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get black list of phone numbers.
        /// </summary>
        /// <returns>The black list phones.</returns>
        public Dictionary<string, object> getBlackListPhones()
        {
            Dictionary<string, object> result = null;
            var url = "/sms/black_list";
            try
            {
                result = sendRequest(url, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Retrieving information of telephone numbers in the blacklist
        /// </summary>
        /// <returns>The phones info in black list.</returns>
        public Dictionary<string, object> getPhonesInfoInBlackList(string phones)
        {
            Dictionary<string, object> result = null;
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            var url = "/sms/black_list/by_numbers";
            try
            {
                result = sendRequest(url, "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Send the sms campaign.
        /// </summary>
        /// <returns>The sms campaign.</returns>
        /// <param name="bookId">Book identifier.</param>
        /// <param name="body">Body.</param>
        /// <param name="transliterate">Transliterate.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="date">Date.</param>
        public Dictionary<string, object> sendSmsCampaign(int bookId, string body, int transliterate = 1,
            string sender = "", string date = "")
        {
            if (body.Length == 0) return handleError("Empty Body");
            if (bookId <= 0) return handleError("Empty address book Id");
            var data = new Dictionary<string, object>();
            data.Add("addressBookId", bookId);
            data.Add("body", body);
            if (sender != null) data.Add("sender", sender);
            data.Add("transliterate", transliterate);
            if (date != null) data.Add("date", date);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/campaigns", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Send sms campaign by phones list.
        /// </summary>
        /// <returns>The sms campaign by phones.</returns>
        /// <param name="phones">Phones.</param>
        /// <param name="body">Body.</param>
        /// <param name="transliterate">Transliterate.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="date">Date.</param>
        public Dictionary<string, object> sendSmsCampaignByPhones(string phones, string body, int transliterate = 1,
            string sender = "", string date = "")
        {
            if (body.Length == 0)
                return handleError("Empty Body");
            if (phones.Length == 0)
                return handleError("Empty phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            data.Add("body", body);
            if (sender != null) data.Add("sender", sender);
            data.Add("transliterate", transliterate);
            if (date != null) data.Add("date", date);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/send", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get sms campaigns list.
        /// </summary>
        /// <returns>The sms campaigns list.</returns>
        /// <param name="dateFrom">Date from.</param>
        /// <param name="dateTo">Date to.</param>
        public Dictionary<string, object> getSmsCampaignsList(string dateFrom, string dateTo)
        {
            Dictionary<string, object> result = null;
            var data = new Dictionary<string, object>();
            data.Add("dateFrom", dateFrom);
            data.Add("dateTo", dateTo);
            var url = "/sms/campaigns/list";
            try
            {
                result = sendRequest(url, "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get sms campaign info.
        /// </summary>
        /// <returns>The sms campaign info.</returns>
        /// <param name="id">Identifier.</param>
        public Dictionary<string, object> getSmsCampaignInfo(int id)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (id > 0)
            {
                url = "/sms/campaigns/info/" + id;
                try
                {
                    result = sendRequest(url, "GET", null);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Cancel sms campaign.
        /// </summary>
        /// <returns>The sms campaign.</returns>
        /// <param name="id">Identifier.</param>
        public Dictionary<string, object> cancelSmsCampaign(int id)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (id > 0)
            {
                url = "/sms/campaigns/cancel/" + id;
                try
                {
                    result = sendRequest(url, "GET", null);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Get sms campaign cost.
        /// </summary>
        /// <returns>The sms campaign cost.</returns>
        /// <param name="body">Body.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="addressBookId">Address book identifier.</param>
        /// <param name="phones">Phones.</param>
        public Dictionary<string, object> getSmsCampaignCost(string body, string sender, int addressBookId = 0,
            string phones = "")
        {
            if (body.Length == 0) return handleError("Empty Body");
            Dictionary<string, object> result = null;
            var data = new Dictionary<string, object>();
            data.Add("body", body);
            data.Add("sender", sender);
            if (addressBookId <= 0 && phones == null)
                return handleError("Empty recipients list");
            if (phones.Length > 0)
                data.Add("phones", phones);
            else
                data.Add("addressBookId", addressBookId);
            var url = "/sms/campaigns/cost";
            try
            {
                result = sendRequest(url, "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Delete sms campaign.
        /// </summary>
        /// <returns>The sms campaign.</returns>
        /// <param name="id">Identifier.</param>
        public Dictionary<string, object> deleteSmsCampaign(int id)
        {
            Dictionary<string, object> result = null;
            var url = "";
            if (id > 0)
            {
                var data = new Dictionary<string, object>();
                data.Add("id", id);
                url = "/sms/campaigns";
                try
                {
                    result = sendRequest(url, "DELETE", data);
                }
                catch (IOException ex)
                {
                    Logger.Error(ex);
                }
                return handleResult(result);
            }
            return handleError("Empty ID");
        }
        /// <summary>
        ///     Add phones to addreess book.
        /// </summary>
        /// <returns>The phones to addreess book.</returns>
        /// <param name="addressBookId">Address book identifier.</param>
        /// <param name="phones">Phones.</param>
        public Dictionary<string, object> addPhonesToAddreessBook(int addressBookId, string phones)
        {
            if (addressBookId <= 0) return handleError("Empty address book id");
            if (phones.Length == 0) return handleError("Empty phones");
            var data = new Dictionary<string, object>();
            data.Add("phones", phones);
            data.Add("addressBookId", addressBookId);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/sms/numbers/variables", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Send viber campaign.
        /// </summary>
        /// <returns>The viber campaign.</returns>
        /// <param name="recipients">Recipients.</param>
        /// <param name="addressBookId">Address book identifier.</param>
        /// <param name="message">Message.</param>
        /// <param name="senderId">Sender identifier.</param>
        /// <param name="additional">Additional identifier.</param>
        /// <param name="messageLiveTime">Message live time.</param>
        /// <param name="sendDate">Send date.</param>
        public Dictionary<string, object> sendViberCampaign(string recipients,
            int addressBookId,
            string message,
            int senderId,
            string additional,
            int messageLiveTime = 60,
            string sendDate = "now")
        {
            if (addressBookId <= 0 && recipients.Length == 0) return handleError("Empty recipients list");
            if (message.Length == 0) return handleError("Empty message");
            if (senderId <= 0) return handleError("Empty sender");
            var data = new Dictionary<string, object>();
            if (recipients.Length > 0)
                data.Add("recipients", recipients);
            else if (addressBookId > 0) data.Add("address_book", addressBookId);
            data.Add("message", message);
            data.Add("sender_id", senderId);
            data.Add("send_date", sendDate);
            data.Add("additional", additional);
            data.Add("message_live_time", messageLiveTime);
            Dictionary<string, object> result = null;
            try
            {
                result = sendRequest("/viber", "POST", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get viber senders list.
        /// </summary>
        /// <returns>The viber senders.</returns>
        public Dictionary<string, object> getViberSenders()
        {
            Dictionary<string, object> result = null;
            var url = "/viber/senders";
            try
            {
                result = sendRequest(url, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get viber tasks list.
        /// </summary>
        /// <returns>The viber tasks list.</returns>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        public Dictionary<string, object> getViberTasksList(int limit = 100, int offset = 0)
        {
            Dictionary<string, object> result = null;
            var data = new Dictionary<string, object>();
            data.Add("limit", limit);
            data.Add("offset", offset);
            var url = "/viber/task";
            try
            {
                result = sendRequest(url, "GET", data);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get viber campaign statistic.
        /// </summary>
        /// <returns>The viber campaign stat.</returns>
        /// <param name="id">Identifier.</param>
        public Dictionary<string, object> getViberCampaignStat(int id)
        {
            Dictionary<string, object> result = null;
            if (id <= 0) return handleError("Empty id");
            var url = "/viber/task/" + id;
            try
            {
                result = sendRequest(url, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get the viber sender info.
        /// </summary>
        /// <returns>The viber sender.</returns>
        /// <param name="id">Identifier.</param>
        public Dictionary<string, object> getViberSender(int id)
        {
            Dictionary<string, object> result = null;
            if (id <= 0) return handleError("Empty id");
            var url = "/viber/senders/" + id;
            try
            {
                result = sendRequest(url, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        /// <summary>
        ///     Get viber task recipients.
        /// </summary>
        /// <returns>The viber task recipients.</returns>
        /// <param name="id">Identifier.</param>
        public Dictionary<string, object> getViberTaskRecipients(int id)
        {
            Dictionary<string, object> result = null;
            if (id <= 0)
                return handleError("Empty id");
            var url = "/viber/task/" + id + "/recipients";
            try
            {
                result = sendRequest(url, "GET", null);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            return handleResult(result);
        }
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string md5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
        /// <summary>
        ///     Form and send request to API service
        /// </summary>
        /// <param name="path">string path</param>
        /// <param name="method">string method</param>
        /// <param name="data">
        ///     <string, object> data
        /// </param>
        /// <param name="useToken">Boolean useToken</param>
        /// <returns>Dictionary<string, object> result data</returns>
        public Dictionary<string, object> sendRequest(string path, string method, Dictionary<string, object> data,
            bool useToken = true)
        {
            string strReturn = null;
            var response = new Dictionary<string, object>();
            try
            {
                var stringdata = "";
                if (data != null && data.Count > 0)
                    stringdata = makeRequestString(data);
                method = method.ToUpper();
                if (method == "GET" && stringdata.Length > 0)
                    path = path + "?" + stringdata;
                var WebReq = (HttpWebRequest)WebRequest.Create(_apiurl + "/" + path);
                WebReq.Method = method;
                if (useToken && _tokenName != null)
                    WebReq.Headers.Add("Authorization", "Bearer " + _tokenName);
                if (method != "GET")
                {
                    var buffer = Encoding.ASCII.GetBytes(stringdata);
                    WebReq.ContentType = "application/x-www-form-urlencoded";
                    WebReq.ContentLength = buffer.Length;
                    var PostData = WebReq.GetRequestStream();
                    PostData.Write(buffer, 0, buffer.Length);
                    PostData.Close();
                }
                try
                {
                    var WebResp = (HttpWebResponse)WebReq.GetResponse();
                    var status = WebResp.StatusCode;
                    response.Add("http_code", (int)status);
                    if ((int)status == 401 && _refreshToken == 0)
                    {
                        _refreshToken += 1;
                        getToken();
                        response = sendRequest(path, method, data, false);
                    }
                    else
                    {
                        var WebResponse = WebResp.GetResponseStream();
                        var _response = new StreamReader(WebResponse);
                        strReturn = _response.ReadToEnd();
                        if (strReturn.Length > 0)
                        {
                            object jo = null;
                            try
                            {
                                jo = JsonConvert.DeserializeObject<object>(strReturn.Trim());
                                if (jo.GetType() == typeof(JObject))
                                    jo = (JObject)jo;
                                else if (jo.GetType() == typeof(JArray))
                                    jo = (JArray)jo;
                            }
                            catch (JsonException jex)
                            {
                                foreach (var item in handleError(jex.Message))
                                    response.Add(item.Key, item.Value);
                            }
                            response.Add("data", jo);
                        }
                    }
                }
                catch (WebException we)
                {
                    Logger.Error(we);
                    var wRespStatusCode = ((HttpWebResponse)we.Response).StatusCode;
                    response.Add("http_code", (int)wRespStatusCode);
                    var WebResponse = ((HttpWebResponse)we.Response).GetResponseStream();
                    var _response = new StreamReader(WebResponse);
                    strReturn = _response.ReadToEnd();
                    response.Add("data", strReturn);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                foreach (var item in handleError(ex.Message))
                    response.Add(item.Key, item.Value);
            }
            return response;
        }
        /// <summary>
        ///     Make post data string
        /// </summary>
        /// <param name="data">Dictionary<string, object> params</param>
        /// <returns>string urlstring</returns>
        private string makeRequestString(Dictionary<string, object> data)
        {
            var requeststring = "";
            foreach (var item in data)
            {
                if (requeststring.Length != 0) requeststring = requeststring + "&";
                requeststring = requeststring + HttpUtility.UrlEncode(item.Key, Encoding.UTF8) + "=" +
                                HttpUtility.UrlEncode(item.Value.ToString(), Encoding.UTF8);
            }
            return requeststring;
        }
        /// <summary>
        ///     Get token and store it
        /// </summary>
        /// <returns>bool</returns>
        private bool getToken()
        {
            var data = new Dictionary<string, object>();
            data.Add("grant_type", "client_credentials");
            data.Add("client_id", _userId);
            data.Add("client_secret", _secret);
            Dictionary<string, object> requestResult = null;
            try
            {
                requestResult = sendRequest("oauth/access_token", "POST", data, false);
            }
            catch (IOException ex)
            {
                Logger.Error(ex);
            }
            if (requestResult == null)
                return false;
            if ((int)requestResult["http_code"] != 200)
                return false;
            _refreshToken = 0;
            var jdata = (JObject)requestResult["data"];
            if (jdata.GetType() == typeof(JObject))
                _tokenName = jdata["access_token"].ToString();
            return true;
        }
        /// <summary>
        ///     Process results
        /// </summary>
        /// <param name="data">Dictionary<string, object> data</param>
        /// <returns>Dictionary<string, object> data</returns>
        private Dictionary<string, object> handleResult(Dictionary<string, object> data)
        {
            if (!data.ContainsKey("data") || data.Count == 0)
                data.Add("data", null);
            if ((int)data["http_code"] != 200)
                data.Add("is_error", true);
            return data;
        }
        /// <summary>
        ///     Process errors
        /// </summary>
        /// <param name="customMessage">String Error message</param>
        /// <returns>Dictionary<string, object> data</returns>
        private Dictionary<string, object> handleError(string customMessage)
        {
            var data = new Dictionary<string, object>();
            data.Add("is_error", true);
            if (customMessage != null && customMessage.Length > 0)
                data.Add("message", customMessage);
            return data;
        }
    }
}
