namespace Services.IdServices
{
	public class IdService : IIdService
	{
		private int _currentId;
		
		public IdService() => 
			_currentId = int.MinValue;

		public int GetNextId ()
		{
			var currentId = _currentId;
			_currentId++;
			return currentId;
		} 
	}
}