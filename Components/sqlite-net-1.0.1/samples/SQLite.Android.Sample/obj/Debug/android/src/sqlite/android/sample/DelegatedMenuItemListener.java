package sqlite.android.sample;


public class DelegatedMenuItemListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.MenuItem.OnMenuItemClickListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onMenuItemClick:(Landroid/view/MenuItem;)Z:GetOnMenuItemClick_Landroid_view_MenuItem_Handler:Android.Views.IMenuItemOnMenuItemClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("SQLite.Android.Sample.DelegatedMenuItemListener, SQLite.Android.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DelegatedMenuItemListener.class, __md_methods);
	}


	public DelegatedMenuItemListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DelegatedMenuItemListener.class)
			mono.android.TypeManager.Activate ("SQLite.Android.Sample.DelegatedMenuItemListener, SQLite.Android.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onMenuItemClick (android.view.MenuItem p0)
	{
		return n_onMenuItemClick (p0);
	}

	private native boolean n_onMenuItemClick (android.view.MenuItem p0);

	java.util.ArrayList refList;
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
