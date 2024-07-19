using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Models;

namespace Tchat.Api.Services
{
    /// <summary>
    /// Declare the methods that the service related to the domain manipulation will implements
    /// </summary>
    /// <typeparam name="T">Domain type</typeparam>
    /// <typeparam name="S">Search arguments type</typeparam>
    public interface IDomainService<T, S>
    {
        /// <summary>
        /// Get all the domain objects
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the list of domain objects
        /// </returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Update a domain object
        /// </summary>
        /// <param name="element">Element to update</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated domain object
        /// </returns>
        Task<T> Update(T element);

        /// <summary>
        /// Create a new domain object
        /// </summary>
        /// <param name="element">Element to create</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the created domain object
        /// </returns>
        Task<T> Create(T element);

        /// <summary>
        /// Search a domain object
        /// </summary>
        /// <param name="search">
        /// Search arguments
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> the domain objects
        /// </returns>
        Task<IEnumerable<T>> Search(S search);

        /// <summary>
        /// Get a domain object by its id
        /// </summary>
        /// <param name="id">
        /// Id of the domain object
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the domain object related to the given id
        /// </returns>
        Task<T> GetById(Guid id);

        /// <summary>
        /// Delete a domain object by its id
        /// </summary>
        /// <param name="id">
        /// Id of the domain object to delete
        /// </param>
        /// <returns></returns>
        Task Delete(Guid id);
    }
}
