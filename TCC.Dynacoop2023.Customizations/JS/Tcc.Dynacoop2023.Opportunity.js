if (typeof (Tcc) == "undefined") { Tcc = {} }
if (typeof (Tcc.Opportunity) == "undefined") { Tcc.Account = {} }

Tcc.Opportunity = {
    OnSave: function (executionContext) {
        var formContext = executionContext.getFormContext();
        var integracao = formContext.getAttribute("tcc2_integracao").getValue();
        if (integracao) {
            formContext.ui.setFormNotification("Oportunidade bloqueada: Não é possivel alterar a oportunidade pois ela é herdada de uma integração", "WARNING", "oppMsg");
            formContext.data.entity.attributes.forEach(
                function (attribute) {
                    var attributeName = attribute.getName();
                    formContext.getControl(attributeName).setDisabled(true);
                }
            );
        }
    },
    OnChangeButton: function (primaryControl) {
        debugger;
        var formContext = primaryControl;
        var data =
        {
            "name": formContext.getAttribute("name").getValue(),
            "purchasetimeframe": formContext.getAttribute("purchasetimeframe").getValue(),
            "budgetamount": formContext.getAttribute("budgetamount").getValue(),
            "purchaseprocess": formContext.getAttribute("purchaseprocess").getValue(),
            "msdyn_forecastcategory": formContext.getAttribute("msdyn_forecastcategory").getValue(),
            "description": formContext.getAttribute("description").getValue(),
            "currentsituation": formContext.getAttribute("currentsituation").getValue(),
            "customerneed": formContext.getAttribute("customerneed").getValue(),
            "proposedsolution": formContext.getAttribute("proposedsolution").getValue(),
            "isrevenuesystemcalculated": formContext.getAttribute("isrevenuesystemcalculated").getValue(),
            "totallineitemamount": formContext.getAttribute("totallineitemamount").getValue(),
            "discountpercentage": formContext.getAttribute("discountpercentage").getValue(),
            "discountamount": formContext.getAttribute("discountamount").getValue(),
            "totalamountlessfreight": formContext.getAttribute("totalamountlessfreight").getValue(),
            "freightamount": formContext.getAttribute("freightamount").getValue(),
            "totaltax": formContext.getAttribute("totaltax").getValue(),
            "totalamount": formContext.getAttribute("totalamount").getValue(),
        }

        if (formContext.getAttribute("parentcontactid").getValue() != null)
            data["parentcontactid@odata.bind"] = "/contacts(" + formContext.getAttribute("parentcontactid").getValue()[0].id.replace("{", "").replace("}", "") + ")"
        if (formContext.getAttribute("parentaccountid").getValue() != null)
            data["parentaccountid@odata.bind"] = "/accounts(" + formContext.getAttribute("parentaccountid").getValue()[0].id.replace("{", "").replace("}", "") + ")"
        if (formContext.getAttribute("transactioncurrencyid").getValue() != null)
            data["transactioncurrencyid@odata.bind"] = "/transactioncurrencys(" + formContext.getAttribute("transactioncurrencyid").getValue()[0].id.replace("{", "").replace("}", "") + ")"
        if (formContext.getAttribute("pricelevelid").getValue() != null)
            data["pricelevelid@odata.bind"] = "/pricelevels(" + formContext.getAttribute("pricelevelid").getValue()[0].id.replace("{", "").replace("}", "") + ")"

        Xrm.WebApi.createRecord("opportunity", data).then(
            function success(result) {
                var idNovaOpp = result.id;

                var gridContext = formContext.getControl("opportunityproductsgrid");

                var myRows = gridContext.getGrid().getRows();

                var RowCount = myRows.getLength();

                for (var i = 0; i < RowCount; i++) {

                    var gridRowData = myRows.get(i).getData();

                    var entity = gridRowData.getEntity();

                    var entityReference = entity.getEntityReference();

                    var fetchXml = "?fetchXml=<fetch>" +
                        "<entity name='opportunityproduct'>" +
                        "<filter>" +
                        "<condition attribute='opportunityproductid' operator='eq' value='" + entityReference.id.replace("{", "").replace("}", "") + "'/>" +
                        "</filter>" +
                        "</entity>" +
                        "</fetch>";

                    Xrm.WebApi.retrieveMultipleRecords("opportunityproduct", fetchXml).then(
                        function success(result) {
                            for (var i = 0; i < result.entities.length; i++) {
                                var dataProd =
                                {
                                    "isproductoverridden": result.entities[i]["isproductoverridden"],
                                    "ispriceoverridden": result.entities[i]["ispriceoverridden"],
                                    "priceperunit": result.entities[i]["priceperunit"],
                                    "volumediscountamount": result.entities[i]["volumediscountamount"],
                                    "quantity": result.entities[i]["quantity"],
                                    "baseamount": result.entities[i]["baseamount"],
                                    "manualdiscountamount": result.entities[i]["manualdiscountamount"],
                                    "tax": result.entities[i]["tax"],
                                    "extendedamount": result.entities[i]["extendedamount"],
                                }

                                if (result.entities[i]["_productid_value"] != null)
                                    dataProd["productid@odata.bind"] = "/products(" + result.entities[i]["_productid_value"].replace("{", "").replace("}", "") + ")"
                                if (result.entities[i]["_uomid_value"] != null)
                                    dataProd["uomid@odata.bind"] = "/uoms(" + result.entities[i]["_uomid_value"].replace("{", "").replace("}", "") + ")"
                               
                                dataProd["opportunityid@odata.bind"] = "/opportunitys(" + idNovaOpp.replace("{", "").replace("}", "") + ")"
                                Xrm.WebApi.createRecord("opportunityproduct", dataProd);
                            }

                        },
                        function (error) {
                            console.log(error.message);
                            // handle error conditions
                        }
                    );
                }
            },
            function (error) {
                console.log(error.message);
                // handle error conditions
            }
        );
        

    },
    DynamicsAlert: function (alertText, alertTitle) {
        var alertStrings = {
            confirmButtonLabel: "OK",
            text: alertText,
            title: alertTitle
        };

        var alertOptions = {
            height: 120,
            width: 200
        };
        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions);
    }
}