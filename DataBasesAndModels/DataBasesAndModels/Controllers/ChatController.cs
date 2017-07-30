using DataBasesAndModels.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBasesAndModels.Controllers
{
    public class ChatController : Controller
    {
        TownContext database = new TownContext();

        int messageBufferSize = 100;
        int userMaxAmount = 10;
        int userMaxPingSec = 30;

        static ChatModel chatModel;

        public ActionResult Index(string user, bool? logOn,bool? logOff, string chatMessage, DateTime? chatMessageTimestamp)
        {
            Debug.WriteLine("Index called!!!");
            try
            {

                if (chatModel == null) chatModel = new ChatModel(messageBufferSize,userMaxAmount);
                 if (!Request.IsAjaxRequest())
                {
                    Debug.WriteLine("Regular request");
                    List<SelectListItem> peopleList;

                    peopleList = database.Characters
                    .Select(c => new SelectListItem()
                    {
                        Text = (c.Name + " " + c.Surname),
                        Value = (c.Name + c.Surname)
                    }).ToList();

                    peopleList.RemoveAll(el => chatModel.Users.FirstOrDefault(usr => usr.Name == el.Value) != null);
                    
                    ViewBag.People = new SelectList(peopleList, "Value", "Text"); ;
                    return View(chatModel);
                }
                else if(logOn != null && (bool)logOn)
                {
                    Debug.WriteLine("Logon request");
                    int logInResult = chatModel.AddUser(user);
                    if (logInResult == ChatModel.NULL_POINTER_RESULT) throw new Exception("Пустое имя пользователя.");
                    if (logInResult == ChatModel.ALREADY_CONNECTED_RESULT) throw new Exception("Данный никнейм уже занят.");
                    if (logInResult == ChatModel.NO_MORE_ROOM_RESULT) throw new Exception("Чат заполнен.");
                    return PartialView("ChatRoom", chatModel);
                }
                else if(logOff != null && (bool)logOff)
                {
                    Debug.WriteLine("Logoff request");
                    chatModel.LogOff(user);
                    return PartialView("ChatRoom", chatModel);
                }
                else
                {
                    Debug.WriteLine("Chat request");
                    ChatUser chu = chatModel.TouchUser(user);

                    chatModel.RemoveDelayedUsers(userMaxPingSec);

                    if (!string.IsNullOrEmpty(chatMessage))
                        if (chatMessageTimestamp != null)
                            chatModel.AddMessage(chatMessage, chu, (DateTime)chatMessageTimestamp);
                        else
                            chatModel.AddMessage(chatMessage, chu);

                    return PartialView("History", chatModel);
                }

            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Content(e.Message);
            }
        }


    }
}