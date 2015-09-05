

    using System;
    using UnityEngine;

public class Relee
{
    private String ID;
    private Color inColor;
    private Color outColor;
    private Boolean closed;

    public Relee(string id, Color inColor, Color outColor)
    {
        ID = id;
        this.inColor = inColor;
        this.outColor = outColor;
    }

    public string Id
    {
        get { return ID; }
    }

    public Color InColor
    {
        get { return inColor; }
    }

    public Color OutColor
    {
        get { return outColor; }
    }

    public bool Closed
    {
        get { return closed; }
        set { closed = value; }
    }


    protected bool Equals(Relee other)
    {
        return string.Equals(ID, other.ID) && inColor.Equals(other.inColor) && outColor.Equals(other.outColor);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Relee) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = (ID != null ? ID.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ inColor.GetHashCode();
            hashCode = (hashCode*397) ^ outColor.GetHashCode();
            return hashCode;
        }
    }
}

