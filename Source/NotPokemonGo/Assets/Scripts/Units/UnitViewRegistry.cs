using System.Collections.Generic;
using UnityEngine;

namespace Units
{
	public class UnitViewRegistry
	{
		private readonly Dictionary<int, UnitDamageView> _views = new();

		public void Register(Unit unit, UnitDamageView view) => 
			_views[unit.Id] = view;

		public void Unregister(Unit unit) => 
			_views.Remove(unit.Id);

		public UnitDamageView Get(Unit unit) => 
			_views.TryGetValue(unit.Id, out UnitDamageView view) ? view : throw new  KeyNotFoundException($"Unit not found. id - {unit.Id}. unitType - {unit.UnitType}");
	}
}