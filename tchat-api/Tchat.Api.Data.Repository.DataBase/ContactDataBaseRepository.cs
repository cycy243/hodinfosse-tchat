using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;
using Tchat.Api.Exceptions.Utils;
using Tchat.API.Args;
using Tchat.API.Persistence;

namespace Tchat.Api.Data.Repository.DataBase
{
    public class ContactDataBaseRepository : IContactRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactDataBaseRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<ContactQuestion> AnswerQuestion(Guid id, string response)
        {
            var question = _dbContext.ContactQuestion.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                throw new RessourceNotFound("Question not found");
            }
            question.Response = response;
            question.DateRespond = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return question;
        }

        public async Task AskQuestion(ContactQuestion question)
        {
            await _dbContext.ContactQuestion.AddAsync(question);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteQuestion(Guid id)
        {
            var question = _dbContext.ContactQuestion.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                throw new RessourceNotFound("No message found for the id: " + id);
            }
            question.DeletedOn = DateTime.Now;
            _dbContext.ContactQuestion.Update(question);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<ContactQuestion>> GetQuestions(ContactQuestionSearchArgs args)
        {
            if (args.Count != null && args.Count < 0)
            {
                throw new InvalidArgumentException("Not valid argument", "The [count] should be inferior thant 0");
            }
            var questions = _dbContext.ContactQuestion.AsEnumerable().Where(cq => cq.IsDeleted == args.IsDeleted).OrderBy(cq => cq.DateSend).AsQueryable();
            if (args.Count != null)
            {
                questions = questions.TakeLast(args.Count.Value);
            }
            return await Task.FromResult(questions);
        }
    }
}
