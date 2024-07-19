using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Models;
using Tchat.API.Args;

namespace Tchat.Api.Services
{
    public interface IContactService
    {
        /// <summary>
        /// Send a mail with a question from a user and save it in the database.
        /// </summary>
        /// <param name="dto">The mail arguments</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AskQuestion(ContactQuestionDto dto);

        /// <summary>
        /// Get all the questions from the database.
        /// </summary>
        /// <param name="count">Wanted count, can be null</param>
        /// <returns>
        /// A task containing the questions.
        /// The retrived question are the last one asked.
        /// If count is null, all the questions are returned.
        /// Otherwise, the count last questions are returned.
        /// </returns>
        Task<IEnumerable<ContactQuestionDto>> GetAllQuestions(ContactQuestionSearchArgs args);

        /// <summary>
        /// Answer a question from the database.
        /// </summary>
        /// <param name="dto">Question with the answer</param>
        /// <returns>
        /// A task containing the answered question.
        /// </returns>
        Task<ContactQuestionDto> AnswerQuestion(ContactQuestionDto dto);

        /// <summary>
        /// Delete a question from the data storage.
        /// </summary>
        /// <param name="id">Id of the question</param>
        /// <returns>
        /// A task containing the result of the operation. True if the message was deleted, false otherwise.
        /// </returns>
        Task<bool> DeleteQuestion(Guid id);
    }
}
