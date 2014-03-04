package sqlite.android.sample;


public class TabListener_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.app.ActionBar.TabListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onTabReselected:(Landroid/app/ActionBar$Tab;Landroid/app/FragmentTransaction;)V:GetOnTabReselected_Landroid_app_ActionBar_Tab_Landroid_app_FragmentTransaction_Handler:Android.App.ActionBar/ITabListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onTabSelected:(Landroid/app/ActionBar$Tab;Landroid/app/FragmentTransaction;)V:GetOnTabSelected_Landroid_app_ActionBar_Tab_Landroid_app_FragmentTransaction_Handler:Android.App.ActionBar/ITabListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onTabUnselected:(Landroid/app/ActionBar$Tab;Landroid/app/FragmentTransaction;)V:GetOnTabUnselected_Landroid_app_ActionBar_Tab_Landroid_app_FragmentTransaction_Handler:Android.App.ActionBar/ITabListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("SQLite.Android.Sample.TabListener`1, SQLite.Android.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TabListener_1.class, __md_methods);
	}


	public TabListener_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TabListener_1.class)
			mono.android.TypeManager.Activate ("SQLite.Android.Sample.TabListener`1, SQLite.Android.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public TabListener_1 (android.content.Context p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == TabListener_1.class)
			mono.android.TypeManager.Activate ("SQLite.Android.Sample.TabListener`1, SQLite.Android.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onTabReselected (android.app.ActionBar.Tab p0, android.app.FragmentTransaction p1)
	{
		n_onTabReselected (p0, p1);
	}

	private native void n_onTabReselected (android.app.ActionBar.Tab p0, android.app.FragmentTransaction p1);


	public void onTabSelected (android.app.ActionBar.Tab p0, android.app.FragmentTransaction p1)
	{
		n_onTabSelected (p0, p1);
	}

	private native void n_onTabSelected (android.app.ActionBar.Tab p0, android.app.FragmentTransaction p1);


	public void onTabUnselected (android.app.ActionBar.Tab p0, android.app.FragmentTransaction p1)
	{
		n_onTabUnselected (p0, p1);
	}

	private native void n_onTabUnselected (android.app.ActionBar.Tab p0, android.app.FragmentTransaction p1);

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
