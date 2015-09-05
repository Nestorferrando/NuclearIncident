using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


 public   class ConnectedRelee
 {

     private Relee relee;
     private int phase;

     public ConnectedRelee(Relee relee, int phase)
     {
         this.relee = relee;
         this.phase = phase;
     }

     public Relee Relee
     {
         get { return relee; }
     }

     public int Phase
     {
         get { return phase; }
     }

     private sealed class ReleeEqualityComparer : IEqualityComparer<ConnectedRelee>
     {
         public bool Equals(ConnectedRelee x, ConnectedRelee y)
         {
             if (ReferenceEquals(x, y)) return true;
             if (ReferenceEquals(x, null)) return false;
             if (ReferenceEquals(y, null)) return false;
             if (x.GetType() != y.GetType()) return false;
             return Equals(x.relee, y.relee);
         }

         public int GetHashCode(ConnectedRelee obj)
         {
             return (obj.relee != null ? obj.relee.GetHashCode() : 0);
         }
     }

     private static readonly IEqualityComparer<ConnectedRelee> ReleeComparerInstance = new ReleeEqualityComparer();

     public static IEqualityComparer<ConnectedRelee> ReleeComparer
     {
         get { return ReleeComparerInstance; }
     }

     protected bool Equals(ConnectedRelee other)
     {
         return Equals(relee, other.relee);
     }

     public override bool Equals(object obj)
     {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != this.GetType()) return false;
         return Equals((ConnectedRelee) obj);
     }

     public override int GetHashCode()
     {
         return (relee != null ? relee.GetHashCode() : 0);
     }
 }

