using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Concrete
{
    public sealed class Message : BaseEntity
    {
        public enum MessageFolder
        {
            Inbox,      // Gelen Kutusu
            Sent,       // Gönderilenler
            Draft,      // Taslaklar
            Trash       // Çöp Kutusu

        }
        //gönderen
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;

        //alıcı
        public string ReceiverEmail { get; set; } = string.Empty;

        //mesaj içeriği
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        //durum
        public bool IsRead { get; set; } = false;
        public bool IsStarred { get; set; } = false;
        public MessageFolder Folder { get; set; } = MessageFolder.Inbox;
    }
}
