namespace Armaments
{
    public interface IArmamentViewFactory
    {
        Armament Create(ArmamentContext context);
    }
}