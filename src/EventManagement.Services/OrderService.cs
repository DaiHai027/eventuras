﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using losol.EventManagement.Domain;
using losol.EventManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace losol.EventManagement.Services
{
	public class OrderService : IOrderService
	{
		private readonly ApplicationDbContext _db;

		public OrderService(ApplicationDbContext db)
		{
			_db = db;
		}

		public Task<List<Order>> GetAsync() =>
			_db.Orders
		       .OrderByDescending(o => o.OrderTime)
		       .ToListAsync();

		public Task<List<Order>> GetAsync(int count, int offset) =>
			_db.Orders
		       .Skip(offset)
		       .Take(count)
		       .OrderByDescending(o => o.OrderTime)
		       .ToListAsync();

		public Task<List<Order>> GetAsync(int count) => GetAsync(count, 0);

		public Task<Order> GetByIdAsync(int orderId) =>
			_db.Orders
			   .Where(o => o.OrderId == orderId)
			   .Include(o => o.OrderLines)
			   .Include(o => o.User)
		       .Include(o => o.Registration)
			   .SingleOrDefaultAsync();

		public Task<OrderLine> GetOrderLineAsync(int lineId) =>
			_db.OrderLines
			   .Where(l => l.OrderLineId == lineId)
		       .Include(l => l.Order)
			   .SingleOrDefaultAsync();

		public async Task<bool> DeleteOrderLineAsync(int lineId)
		{
			var line = await GetOrderLineAsync(lineId);
			if(line == null)
			{
				throw new ArgumentException("Invalid lineId", nameof(lineId));
			}

			if(!line.Order.CanEdit)
			{
				throw new InvalidOperationException("The order line cannot be edited anymore.");
			}

			_db.OrderLines.Remove(line);
			return await _db.SaveChangesAsync() > 0;
		}

		public async Task<bool> AddOrderLineAsync(int orderId, int productId, int? variantId)
		{
			var order = await _db.Orders.FindAsync(orderId);
			var product = await _db.Products.FindAsync(productId);
			var variant = variantId.HasValue ? await _db.Products.FindAsync(variantId) : null;

			_ = order ?? throw new ArgumentException("Invalid orderId", nameof(orderId));
			_ = product ?? throw new ArgumentException("Invalid productId", nameof(productId));

			var line = new OrderLine
			{
				OrderId = orderId,
				ProductId = productId,
				ProductVariantId = variantId,

				Price = variant?.Price ?? product.Price,
				VatPercent = variant?.VatPercent ?? product.VatPercent,

				ProductName = product.Name,
				ProductDescription = product.Description,
				ProductVariantName = variant?.Name,
				ProductVariantDescription = variant?.Description
			};
			await _db.OrderLines.AddAsync(line);
			return await _db.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateOrderLine(int lineId, int quantity, decimal price)
		{
			var line = await _db.OrderLines.FindAsync(lineId);
			_ = line ?? throw new ArgumentException("Invalid lineId", nameof(lineId));

			line.Quantity = quantity;
			line.Price = price;

			_db.Update(line);
			return await _db.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateOrderDetailsAsync(int id, string customername, string customerEmail, string invoiceReference, string comments)
		{
			var order = await _db.Orders.FindAsync(id);

			order.CustomerName = customername;
			order.CustomerEmail = customerEmail;
			order.CustomerInvoiceReference = invoiceReference;
			order.Comments = comments;

			_db.Orders.Update(order);
			return await _db.SaveChangesAsync() > 0;
		}
	}
}