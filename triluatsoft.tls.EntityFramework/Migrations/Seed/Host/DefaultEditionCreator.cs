using System.Linq;
using Abp.Application.Editions;
using Abp.Application.Features;
using triluatsoft.tls.Editions;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.Features;

namespace triluatsoft.tls.Migrations.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly tlsDbContext _context;

        public DefaultEditionCreator(tlsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                //TODO: Add desired features to the standard edition, if wanted!
            }

            if (defaultEdition.Id > 0)
            {
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.ChatFeature, false);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToTenantChatFeature, false);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToHostChatFeature, false);
            }
        }

        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            var defaultEditionChatFeature = _context.EditionFeatureSettings
                                                        .FirstOrDefault(ef => ef.EditionId == editionId && ef.Name == featureName);

            if (defaultEditionChatFeature == null)
            {
                _context.EditionFeatureSettings.Add(new EditionFeatureSetting
                {
                    Name = featureName,
                    Value = isEnabled.ToString(),
                    EditionId = editionId
                });
            }
        }
    }
}