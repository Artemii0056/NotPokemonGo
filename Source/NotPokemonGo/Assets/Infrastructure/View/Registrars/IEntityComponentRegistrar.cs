﻿namespace Infrastructure.View.Registrars
{
    public interface IEntityComponentRegistrar
    {
        void RegisterComponents();
        void UnregisterComponents();
    }
}