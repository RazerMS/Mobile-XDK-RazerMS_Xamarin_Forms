<!--
# license: Copyright © 2011-2016 MOLPay Sdn Bhd. All Rights Reserved. 
-->

# molpay-mobile-xdk-xamarin-forms

This is the complete and functional MOLPay Xamarin Forms payment module that is ready to be implemented into Xamarin Forms project through simple copy and paste procedures. An example application project (MPayXDKExample) is provided for MOLPayXDK Xamarin Forms integration reference.

This plugin provides an integrated MOLPay payment module that contains a wrapper 'MOLPayXDK.cs' and an upgradable core as the 'molpay-mobile-xdk-www' folder, which the latter can be separately downloaded at https://github.com/MOLPay/molpay-mobile-xdk-www and update the local version.

## Recommended configurations

    - Xamarin Studio Version: 6 ++ (For Mac)
    
    - Microsoft Visual Studio 2015 (v14) (For Windows)
    
    - Minimum Android target version: Android 4.1
    
    - Minimum iOS target version: 7.0

## MOLPay Android Caveats

    Credit card payment channel is not available in Android 4.1, 4.2, and 4.3. due to lack of latest security (TLS 1.2) support on these Android platforms natively.

## Installation

    Step 1 - Import MOLPay modules
    Add MOLPayXDK.cs into the FORMS project folder
    For Android, add molpay-mobile-xdk-www into Assets folder, then use Build Action option at all sub files and set as AndroidAsset.
    For iOS, add molpay-mobile-xdk-www into Resources folder.
    
    Step 2 - Add Package dependancies
    Add Json.NET from Official NuGet Gallery
    
    Step 3 - Import Namespaces, add following statements
    using System.Collections.Generic;
    using MOLPayXDK;
    
    Step 4 - Add callback function for transaction results,
    private void MolpayCallback(string transactionResult) {}
    
    Step 5 - Implement native assetsPath,
    LoadApplication (new App ("file:///android_asset/")); (For Android)
    LoadApplication (new App (NSBundle.MainBundle.BundlePath)); (For iOS)
    
    Step 6 - Add 'NSAppTransportSecurity' > Allow Arbitrary Loads > YES' to the application project info.plist (For iOS)
    
    Step 7 - Restore Android platform packages is necessary (Optional) 

## Payment module callback

    private void MolpayCallback(string transactionResult)
        {
            System.Diagnostics.Debug.WriteLine("transactionResult = {0}", transactionResult);
        }
    
    =========================================
    Sample transaction result in JSON string:
    =========================================
    
    {"status_code":"11","amount":"1.01","chksum":"34a9ec11a5b79f31a15176ffbcac76cd","pInstruction":0,"msgType":"C6","paydate":1459240430,"order_id":"3q3rux7dj","err_desc":"","channel":"Credit","app_code":"439187","txn_ID":"6936766"}
    
    Parameter and meaning:
    
    "status_code" - "00" for Success, "11" for Failed, "22" for *Pending. 
    (*Pending status only applicable to cash channels only)
    "amount" - The transaction amount
    "paydate" - The transaction date
    "order_id" - The transaction order id
    "channel" - The transaction channel description
    "txn_ID" - The transaction id generated by MOLPay
    
    * Notes: You may ignore other parameters and values not stated above
    
    =====================================
    * Sample error result in JSON string:
    =====================================
    
    {"Error":"Communication Error"}
    
    Parameter and meaning:
    
    "Communication Error" - Error starting a payment process due to several possible reasons, please contact MOLPay support should the error persists.
    1) Internet not available
    2) API credentials (username, password, merchant id, verify key)
    3) MOLPay server offline.

## Prepare the Payment detail object

    var paymentDetails = new Dictionary<string, object> {
        // Mandatory String. A value more than '1.00'
        { "mp_amount", "1.10" },
    
        // Mandatory String. Values obtained from MOLPay
        { "mp_username", "" },
        { "mp_password", "" },
        { "mp_merchant_ID", "" },
        { "mp_app_name", "" },
        { "mp_verification_key", "" },  
    
        // Mandatory String. Payment values
        { "mp_order_ID", "" }, 
        { "mp_currency", "MYR" },
        { "mp_country", "MY" },  
            
        // Optional String.
        { "mp_channel", "" }, // Use 'multi' for all available channels option. For individual channel seletion, please refer to "Channel Parameter" in "Channel Lists" in the MOLPay API Spec for Merchant pdf. 
        { "mp_bill_description", "" },
        { "mp_bill_name", "" },
        { "mp_bill_email", "" },
        { "mp_bill_mobile", "" },
        { "mp_channel_editing", false }, // Option to allow channel selection.
        { "mp_editing_enabled", false }, // Option to allow billing information editing.
            
        // Optional for Escrow
        { "mp_is_escrow", "" }, // Optional for Escrow, put "1" to enable escrow
    
        // Optional for credit card BIN restrictions
        { "mp_bin_lock", new string[]{"414170", "414171"} }, // Optional for credit card BIN restrictions
        { "mp_bin_lock_err_msg", "Only UOB allowed" }, // Optional for credit card BIN restrictions
            
        // For transaction request use only, do not use this on payment process
        { "mp_transaction_id", "" }, // Optional, provide a valid cash channel transaction id here will display a payment instruction screen.
        { "mp_request_type", "" } // Optional, set 'Status' when performing a transactionRequest
    };

## Start the payment module

    Step 1 - Initiate MOLPay payment module, pass in required parameters below
    var molpay = new MOLPay(assetsPath, paymentDetails, MolpayCallback);
    
    Step 2 - Add MOLPay payment UI to the main layout
    mainLayout.Children.Add(molpay);

## Close the payment module UI

    molpay.CloseMolpay();
    
    * Notes: closeMolpay does not close remove the UI, the host application must implement it's own mechanism to close the payment module UI, 
    
    * Example: Implementing MOLPay closing mechanism at host app
    private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            this.molpay.CloseMolpay();
            mainLayout.Children.Remove(this.molpay);
        }
    
    * The close event will also return a final result.

## Cash channel payment process (How does it work?)

    This is how the cash channels work on XDK:
    
    1) The user initiate a cash payment, upon completed, the XDK will pause at the “Payment instruction” screen, the results would return a pending status.
    
    2) The user can then click on “Close” to exit the MOLPay XDK aka the payment screen.
    
    3) When later in time, the user would arrive at say 7-Eleven to make the payment, the host app then can call the XDK again to display the “Payment Instruction” again, then it has to pass in all the payment details like it will for the standard payment process, only this time, the host app will have to also pass in an extra value in the payment details, it’s the “mp_transaction_id”, the value has to be the same transaction returned in the results from the XDK earlier during the completion of the transaction. If the transaction id provided is accurate, the XDK will instead show the “Payment Instruction" in place of the standard payment screen.
    
    4) After the user done the paying at the 7-Eleven counter, they can close and exit MOLPay XDK by clicking the “Close” button again.

## Support

Submit issue to this repository or email to our support@molpay.com

Merchant Technical Support / Customer Care : support@molpay.com<br>
Sales/Reseller Enquiry : sales@molpay.com<br>
Marketing Campaign : marketing@molpay.com<br>
Channel/Partner Enquiry : channel@molpay.com<br>
Media Contact : media@molpay.com<br>
R&D and Tech-related Suggestion : technical@molpay.com<br>
Abuse Reporting : abuse@molpay.com