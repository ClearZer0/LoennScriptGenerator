using Celeste.Mod.Entities;
using Celeste.Mod.LoennScriptGenerator.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.Entities;

[CustomEntity("LoennScriptGenerator/Test")]
public class TestEntity : Entity
{
    [PlacementsElement]
    public int Dash = 1;
}
