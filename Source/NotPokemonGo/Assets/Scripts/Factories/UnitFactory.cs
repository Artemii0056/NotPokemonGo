using System;
using Characters;
using Characters.Configs;
using Units;
using Object = UnityEngine.Object;

namespace Factories
{
    public class UnitFactory
    {
        private readonly UnitView _unitView;

        public UnitFactory(UnitView unitView)
        {
            _unitView = unitView;
        }

        public UnitView Create(CharacterType characterType,  CharacterStaticData config)
        {
            UnitView unit = null;

            switch (characterType)
            {
                case CharacterType.First:
                    unit = Object.Instantiate(_unitView);
                    UnitModel model = new UnitModel(config.Stats);
                    UnitPresenter presenter = new UnitPresenter(unit, model);
                    presenter.OnEnable();
                    break;

                case CharacterType.Second:

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(characterType), characterType, null);
            }

            return unit;
        }
    }

}