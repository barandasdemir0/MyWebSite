using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfMessageDal : GenericRepository<Message>, IMessageDal
    {
        public EfMessageDal(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<Message> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            IQueryable<Message> messages = _context.Messages.AsNoTrackingWithIdentityResolution(); //sorguyu oluştur context.messages sonrası aşağıda bu sorgunun başı
            var totalCount = await messages.CountAsync(cancellationToken);//toplam sayıyı hesapla select count(*) from messages gibi

            var items = await messages
                .OrderByDescending(x => x.CreatedAt) //bunları sırala ama tersine yani en yeni mesaj en üstte
                .Skip((page - 1) * size) //önceki sayfaları atla demek bu 
                .Take(size) // her sayfada gösterilecek kayıtı al
                .ToListAsync(cancellationToken); //ve listele

            return (items, totalCount);
        }
    }
}
