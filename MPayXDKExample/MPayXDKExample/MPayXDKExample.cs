using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace MPayXDKExample
{
	public class App : Application
	{
		private MOLPay molpay = null;
		private StackLayout mainLayout = null;

		public App()
		{
			var closeButton = new Button();
			closeButton.BackgroundColor = Color.Gray;
			closeButton.Text = "Close";
			closeButton.BorderRadius = 0;
			closeButton.TextColor = Color.White;
			closeButton.VerticalOptions = LayoutOptions.FillAndExpand;
			closeButton.HorizontalOptions = LayoutOptions.FillAndExpand;
			closeButton.Clicked += OnCloseButtonClicked;

			var buttonLayout = new StackLayout
			{
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				HeightRequest = 40,
				Children = {
					closeButton
				}
			};

			this.mainLayout = new StackLayout
			{
				BackgroundColor = Color.Blue,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			this.mainLayout.Children.Add(buttonLayout);

			var paymentDetails = new Dictionary<string, object>
			{
				// ------- SDK required data ----------
				{ "mp_amount", "1.10" }, // Mandatory String. A value more than '1.00'
				{ "mp_username", "" }, // Mandatory String. Values obtained from MOLPay
				{ "mp_password", "" }, // Mandatory String. Values obtained from MOLPay
				{ "mp_merchant_ID", "" }, // Mandatory String. Values obtained from MOLPay
				{ "mp_app_name", "" }, // Mandatory String. Values obtained from MOLPay
				{ "mp_order_ID", "" }, // Mandatory String. Payment values
				{ "mp_currency", "MYR" }, // Mandatory String. Payment values
				{ "mp_country", "MY" }, // Mandatory String. Payment values
				{ "mp_verification_key", "" }, // Mandatory String. Values obtained from MOLPay
				{ "mp_channel", "" }, // Optional String.
				{ "mp_bill_name", "" }, // Optional String.
				{ "mp_bill_email", "" }, // Optional String.
				{ "mp_bill_mobile", "" }, // Optional String.
				{ "mp_bill_description", "" } // Optional String.
				//{ "mp_channel_editing", false }, // Optional String.
				//{ "mp_editing_enabled", false }, // Optional String.
				//{ "mp_transaction_id", "" }, // For transaction request use only, do not use this on payment process
				//{ "mp_request_type", "" }, // Optional, set 'Status' when performing a transactionRequest
				//{ "mp_preferred_token", "" } // Optional, set the token id to nominate a preferred token as the default selection
				//{ "mp_bin_lock", new string[]{"414170", "414171"} }, // Optional for credit card BIN restrictions
				//{ "mp_bin_lock_err_msg", "Only UOB allowed" }, // Optional for credit card BIN restrictions
				//{ "mp_is_escrow", "" }, // Optional for Escrow, put "1" to enable escrow
				//{ "mp_filter", "" }, // Optional for debit card payment only 
				//{ "mp_custom_css_url", System.IO.Path.Combine (assetsPath, "custom.css") }, // Optional for custom UI
				//{ "mp_tcctype", "" }, // Optional, credit card transaction type, set "AUTH" to authorize the transaction
				//{ "mp_is_recurring", false } // Optional, set true to process this transaction through the recurring api, please refer the MOLPay Recurring API pdf
				//{ "mp_allowed_channels", new string[]{"credit", "credit3"} }, // Optional for channels restriction
				//{ "mp_sandbox_mode", true }, // Optional for sandboxed development environment, set boolean value to enable.
				//{ "mp_express_mode", true }, // Optional, required a valid mp_channel value, this will skip the payment info page and go direct to the payment screen.
				//{ "mp_advanced_email_validation_enabled", true }, // Optional, enable this for extended email format validation based on W3C standards.
				//{ "mp_advanced_phone_validation_enabled", true } // Optional, enable this for extended phone format validation based on Google i18n standards.
			};

			this.molpay = new MOLPay(DependencyService.Get<MOLPayExtension>().GetAssetPath(), paymentDetails, MolpayCallback);

			this.mainLayout.Children.Add(this.molpay);

			// The root page of your application
			MainPage = new ContentPage
			{
				Content = this.mainLayout
			};

		}

		private void MolpayCallback(string transactionResult)
		{
			System.Diagnostics.Debug.WriteLine("MOLPayXDK Debug OK >>>>>>>> molpayCallback, transactionResult = {0}", transactionResult);
			// this.mainLayout.Children.Remove(this.molpay);
		}

		private void OnCloseButtonClicked(object sender, EventArgs e)
		{
			this.molpay.CloseMolpay();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

