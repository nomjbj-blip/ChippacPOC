using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SP.Common
{
    public delegate void AnalysisCreatedHandler(Analysis project);
    public delegate void AnalysisRenamedHandler(string name);

    public delegate void EntityAddedHandler(Entity entity);
    public delegate void EntityRemovedHandler(Entity entity);
    public delegate void EntityRenamedHandler(string name);
    public delegate void EntityNameDuplicatedHandler(string inputName, string alterName);


}
