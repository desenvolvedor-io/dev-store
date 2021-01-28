using System;
using FluentValidation;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands
{
    public class RegistrarClienteCommand : Command
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegistrarClienteCommand(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        public override bool EhValido()
        {
            ValidationResult = new RegistrarClienteValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RegistrarClienteValidation : AbstractValidator<RegistrarClienteCommand>
        {
            public RegistrarClienteValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("O nome do cliente não foi informado");

                RuleFor(c => c.Cpf)
                    .Must(TerCpfValido)
                    .WithMessage("O CPF informado não é válido.");

                RuleFor(c => c.Email)
                    .Must(TerEmailValido)
                    .WithMessage("O e-mail informado não é válido.");
            }

            protected static bool TerCpfValido(string cpf)
            {
                return Core.DomainObjects.Cpf.Validar(cpf);
            }

            protected static bool TerEmailValido(string email)
            {
                return Core.DomainObjects.Email.Validar(email);
            }
        }
    }
}