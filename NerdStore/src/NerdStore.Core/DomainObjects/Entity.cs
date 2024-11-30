using System;
using System.Collections.Generic;
using NerdStore.Core.Messages;

namespace NerdStore.Core.DomainObjects
{
    // classe de marcação para identificar que é uma entidade
    // abstract para não ser instanciada
    public abstract class Entity
    {
        // Guid é um identificador único global
        public Guid Id { get; set; }

        private List<Event> _notificacoes;
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        // construtor protegido para que não seja instanciado fora da classe
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }

        // compare entities (comparing IDs)
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        // C# 12
        // public override bool Equals(object obj)
        // {
        //     if (obj is not Entity compareTo) return false;
        //     if (ReferenceEquals(this, compareTo)) return true;

        //     return Id.Equals(compareTo.Id);
        // }

        // compare entities (comparing IDs)
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        // generating a random hashCode.
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        // objective: to show the entity name and its ID
        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        // entity must validate itself
        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}