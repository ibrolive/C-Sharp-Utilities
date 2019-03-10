using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Utilities.Test
{
    [TestClass]
    public class EmailHelperTest
    {
        [TestMethod]
        public void TestSendMail_SuccessfullyWithoutAwait()
        {
            string RecipientEmail = "youremail@domain.com";
            string BccEmail = "youremail@domain.com;youremail@domain.com";
            string Subject = "UnitTest: MAIL SENT WITHOUT AWAIT";
            string Body = "this is the body of the email. some message here.";

            EmailHelper.Message message = new EmailHelper.Message(RecipientEmail, BccEmail, Subject, Body);

            EmailHelper emailHelper = new EmailHelper();
            emailHelper.SendMail(JsonConvert.SerializeObject(message));
        }

        [TestMethod]
        public async Task TestSendMail_SuccessfullyWithAwait()
        {
            string RecipientEmail = "youremail@domain.com";
            string BccEmail = "youremail@domain.com;youremail@domain.com";
            string Subject = "UnitTest: NEW MAIL ADDED TO QUEUE";
            string Body = "this is the body of the email. some message here.";

            EmailHelper.Message message = new EmailHelper.Message(RecipientEmail, BccEmail, Subject, Body);

            EmailHelper emailHelper = new EmailHelper();
            bool successStatus = await emailHelper.SendMail(JsonConvert.SerializeObject(message));
            Assert.IsTrue(successStatus);
        }

        [TestMethod]
        public async Task TestAddEmailToQueue_SuccessfullyWithAwait()
        {
            string RecipientEmail = "youremail@domain.com";
            string BccEmail = "youremail@domain.com;youremail@domain.com";
            string Subject = "UnitTest: NEW MAIL ADDED TO QUEUE";
            string Body = "this is the body of the email. some message here.";

            EmailHelper.Message message = new EmailHelper.Message(RecipientEmail, BccEmail, Subject, Body);

            EmailHelper emailHelper = new EmailHelper();

            bool successStatus = await emailHelper.AddEmailToQueue(message);
            Assert.IsTrue(successStatus);
        }

        [TestMethod]
        public async Task TestSendMail_SuccessfullyWithAwait_WithoutBccEmail()
        {
            string RecipientEmail = "youremail@domain.com";
            string Subject = "UnitTest: MAIL SENT WITH AWAIT - WITH NO BCC";
            string Body = "this is the body of the email. some message here.";

            EmailHelper.Message message = new EmailHelper.Message(RecipientEmail, Subject, Body);

            EmailHelper emailHelper = new EmailHelper();
            bool successStatus = await emailHelper.SendMail(JsonConvert.SerializeObject(message));
            Assert.IsTrue(successStatus);
        }

        [TestMethod]
        public async Task TestSendMail_SuccessfullyWithAwait_WithTemplate()
        {
            string RecipientEmail = "youremail@domain.com";
            string Subject = "UnitTest: MAIL SENT WITH AWAIT - WITH TEMPLATE";
            string Body = new EmailTemplateHelper().GetTemplateContent("Sample-Template");

            EmailHelper.Message message = new EmailHelper.Message(RecipientEmail, Subject, Body);

            EmailHelper emailHelper = new EmailHelper();
            bool successStatus = await emailHelper.SendMail(JsonConvert.SerializeObject(message));
            Assert.IsTrue(successStatus);
        }
    }
}