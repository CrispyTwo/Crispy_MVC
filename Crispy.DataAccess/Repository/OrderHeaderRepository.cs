using Crispy.DataAccess.Data;
using Crispy.DataAccess.Repository.IRepository;
using Crispy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crispy.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public readonly ApplicationDBContext _db;
        public OrderHeaderRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }
        public void Update(OrderHeader obj)
        {
            _db.OrderHeader.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIndentId)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIndentId))
            {
                orderFromDb.PaymentIntendId = paymentIndentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
