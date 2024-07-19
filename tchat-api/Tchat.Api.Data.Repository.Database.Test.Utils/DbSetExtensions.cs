using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;

namespace Tchat.Api.Data.Repository.Database.Test.Utils
{
    public static class DbSetExtensions
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(this List<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSetMock.Setup(m => m.AddAsync(It.IsAny<T>(), default)).Returns<T, System.Threading.CancellationToken>((entity, _) =>
            {
                elements.Add(entity);
                return new ValueTask<EntityEntry<T>>((EntityEntry<T>)null);
            });

            return dbSetMock;
        }

        public static Mock<DbSet<T>> CreateMockDbSetReadable<T>(List<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSetMock.Setup(m => m.AddAsync(It.IsAny<T>(), default)).Returns<T, System.Threading.CancellationToken>((entity, _) =>
            {
                elements.Add(entity);
                return new ValueTask<EntityEntry<T>>((EntityEntry<T>)null);
            });

            return dbSetMock;
        }
    }
}
