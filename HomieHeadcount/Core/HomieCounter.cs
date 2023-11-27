using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HomieHeadcount {
  public class HomieCounter {
    public static List<Tameable> _activeHomies = new();

    public static List<Tameable> GetHomies() {
      return _activeHomies;
    }

    public static void Add(Tameable skeleton) {
      _activeHomies.Add(skeleton);
    }

    public static void Remove(Tameable skeleton) {
      _activeHomies.Remove(skeleton);
    }

    public static bool Contains(Tameable skeleton) {
      return _activeHomies.Contains(skeleton);
    }

    public static int Count () {
      return _activeHomies.Count;
    }
  }
}
