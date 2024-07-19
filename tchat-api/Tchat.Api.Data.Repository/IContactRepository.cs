using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;
using Tchat.API.Args;

namespace Tchat.Api.Data.Repository
{
    public interface IContactRepository
    {
        /// <summary>
        /// Send a mail with a question from a user and save it in the database.
        /// </summary>
        /// <param name="question">The mail arguments</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AskQuestion(ContactQuestion question);

        /// <summary>
        /// Get all the questions from the database.
        /// </summary>
        /// <returns>
        /// A task containing the questions.
        /// </returns>
        Task<IEnumerable<ContactQuestion>> GetQuestions(ContactQuestionSearchArgs args);

        /// <summary>
        /// Answer a question from the database.
        /// </summary>
        /// <param name="id">The question id</param>
        /// <param name="response">The response to the question</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task<ContactQuestion> AnswerQuestion(Guid id, string response);


        /// <summary>
        /// Delete a question from the database.
        /// </summary>
        /// <param name="id">Id of the message to delete</param>
        /// <returns>
        /// A task containing the result of the operation. True if the message was deleted, false otherwise.
        /// </returns>
        Task<bool> DeleteQuestion(Guid id);
    }
}
