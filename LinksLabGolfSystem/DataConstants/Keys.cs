using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.DataConstants {
    public class Keys {
        //THESE **SHOULD** be stored somewhere more secure than here, but for ease of showcase I keep them here.
        //Should this GitHub be made public, I would remove this from the repo and find an alternative way to store it.
		public const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=LinksLabGolfSystem;Trusted_Connection=True;";
        public const string EncryptionKey = @"4VCHS3KUFMMW0G4VQ46G48XNA5TNLSRM";
        public const string SyncFusionKey = "Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX1fd3ZWRmleUUZ0WUs=";

    }
}
