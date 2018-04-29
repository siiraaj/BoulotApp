package md59ea80166094cb9e443c995df2b8b8b72;


public class ProfilActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Boulot.ProfilActivity, Boulot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ProfilActivity.class, __md_methods);
	}


	public ProfilActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ProfilActivity.class)
			mono.android.TypeManager.Activate ("Boulot.ProfilActivity, Boulot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
