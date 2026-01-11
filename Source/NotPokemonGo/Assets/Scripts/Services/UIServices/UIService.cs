using System;
using System.Collections.Generic;
using UI.BaseUI.Interfaces;

namespace Services.UIServices
{
	public class UIService : IUIService, IPresenterRegistrar
	{
		private readonly Dictionary<Type, IPresenter> _presenters = new();

		public void RegisterPresenter<T>(T presenter) where T : IPresenter
		{
			if (_presenters.ContainsKey(typeof(T)))
				throw new ArgumentException($"Presenter {typeof(T)} does exist");

			_presenters.Add(typeof(T), presenter);
		}

		public void UnregisterPresenter<T>() where T : IPresenter
		{
			if (_presenters.ContainsKey(typeof(T)) == false)
				throw new ArgumentException($"Presenter {typeof(T)} does not exist");

			_presenters.Remove(typeof(T));
		}

		public void Show<T>() where T : IPresenter
		{
			if (_presenters.TryGetValue(typeof(T), out var presenter) == false)
				throw new ArgumentException($"Presenter {typeof(T)} does not exist");

			presenter.Activate();
		}

		public void Close<T>() where T : IPresenter
		{
			if (_presenters.TryGetValue(typeof(T), out var presenter) == false)
				throw new ArgumentException($"Presenter {typeof(T)} does not exist");

			presenter.Deactivate();
		}

		public void CloseAll()
		{
			foreach (var presenter in _presenters.Values)
				presenter.Deactivate();
		}

		public void Clear()
		{
			foreach (var presenter in _presenters.Values) 
				presenter.Dispose();
			
			_presenters.Clear();
		}
	}
}