public static class GameManagerSingletonHelper
{

    public static bool CheckOrganismNull(ref Organism organism)
    {

        if (organism == null)
        {

            organism = GameManagerSingleton.Instance.Organism;

        }

        return organism == null;

    }

}
