
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Tchat.Api.Data.Repository;
using Tchat.Api.Domain;
using Tchat.Api.Exceptions.Domain;
using Tchat.Api.Models;
using Tchat.API.Args;

namespace Tchat.Api.Services.Contact
{
    public class ContactService : IContactService
    {
        private readonly IValidator<ContactQuestionDto> _validator;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public ContactService(IValidator<ContactQuestionDto> validator, IContactRepository contactRepository, IMapper mapper, IEmailSender emailSender)
        {
            _validator = validator;
            _contactRepository = contactRepository;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<ContactQuestionDto> AnswerQuestion(ContactQuestionDto dto)
        {
            var errors = await _validator.ValidateAsync(dto);
            if (errors != null && !errors.IsValid)
            {
                throw new ArgumentException(errors.Errors.First().ErrorMessage);
            }
            var question = (await _contactRepository.GetQuestions(new ContactQuestionSearchArgs())).FirstOrDefault(q => q.Id == dto.Id);
            if(question == null)
            {
                throw new Exceptions.Utils.RessourceNotFound($"The question with the id [{dto.Id}] doesn't exist");
            }
            if(question.IsDeleted)
            {
                throw new Exceptions.Domain.InvalidOperationException("The question has been deleted");
            }
            var resultDto = _mapper.Map<ContactQuestion>(await _contactRepository.AnswerQuestion(dto.Id, dto.Response!));
            _emailSender.SendEmail(new MailArg(dto.SenderEmail, "Tracking of your contact - " + dto.Subject, $"You have a response for your message: \n{dto.Content}\nIts response: \n{dto.Response}"));
            _emailSender.SendEmail(new MailArg(
                "developpement.test.mail@gmail.com",
                "Response send for - " + dto.Subject,
                $"A response for subject \"{dto.Subject}\" has been sent.\n The response contains the following content: \n{dto.Content}"));
            return dto;
        }

        public async Task AskQuestion(ContactQuestionDto dto)
        {
            var errors = await _validator.ValidateAsync(dto);
            if (errors != null && !errors.IsValid)
            {
                throw new ArgumentException(errors.Errors.First().ErrorMessage);
            }
            await _contactRepository.AskQuestion(_mapper.Map<ContactQuestion>(dto));
            _emailSender.SendEmail(new MailArg(dto.SenderEmail, "Tracking of your contact - " + dto.Subject, "Thanks for your message. We'll watch it as soon as possible and come back to you.\n\nYour message: \n" + dto.Content));
            _emailSender.SendEmail(new MailArg(
                "developpement.test.mail@gmail.com", 
                "You've got a new message - " + dto.Subject, 
                $"You've got a new message from {dto.SenderName} {dto.SenderFirstname}({dto.SenderEmail}) \n\nThe message: \n{dto.Content}"));
        }

        public async Task<bool> DeleteQuestion(Guid id)
        {
            return await _contactRepository.DeleteQuestion(id);
        }

        public async Task<IEnumerable<ContactQuestionDto>> GetAllQuestions(ContactQuestionSearchArgs args)
        {
            return (await _contactRepository.GetQuestions(new ContactQuestionSearchArgs())).Select(_mapper.Map<ContactQuestion, ContactQuestionDto>);
        }
    }
}
