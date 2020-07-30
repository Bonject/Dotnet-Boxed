namespace GraphQLTemplate.Types
{
    using System;
    using GraphQLTemplate.Models;
    using HotChocolate.Types;

    public class EpisodeEnumeration : EnumType<Episode>
    {
        protected override void Configure(IEnumTypeDescriptor<Episode> descriptor)
        {
            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            descriptor.Name("Episode");
            descriptor.Description("One of the films in the Star Wars Trilogy.");

            descriptor.Value(Episode.NEWHOPE).Description("Released in 1977.");
            descriptor.Value(Episode.EMPIRE).Description("Released in 1980.");
            descriptor.Value(Episode.JEDI).Description("Released in 1983.");
        }
    }
}
