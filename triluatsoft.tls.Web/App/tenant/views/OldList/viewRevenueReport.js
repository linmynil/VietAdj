(function () {
    appModule.controller('tenant.views.OldList.viewRevenueReport', [
        '$scope',
        '$uibModal',
        'abp.services.app.customer',
        function (
            $scope,
            $uibModal,
            customerService
            ) {
            var vm = this;
            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };
            let input = {};
            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
                customerService.getByType('B')
                    .then(function (result) {
                        vm.listBrokers = result.data;
                    });
            };

            vm.getPostData = function () {
                debugger
                input.fromDate = moment(vm.dateRangeModel.startDate).format('MM-DD-YYYY');
                input.toDate = moment(vm.dateRangeModel.endDate).format('MM-DD-YYYY');
                //vm.invoiceCode = vm.invoiceCode;
                input.isAdvInvoice = vm.isAdvInvoice;
                input.brokerID = (vm.brokerID === undefined) ? null : vm.brokerID;
                input.insurerID = (vm.insurerID === undefined) ? null : vm.insurerID;
                input.officeID = vm.officeID;
                input.claimID = (vm.claimID === undefined) ? "" : vm.claimID;
                input.invoiceCode = (vm.invoiceCode === undefined) ? "" : vm.invoiceCode;
                input.lang = vm.lang;
                return input;

            }


            vm.printReport = function (event, printType) {
                debugger
                event.preventDefault();
                vm.getPostData();              

                if (printType === 'inline') {
                    App.startPageLoading({ animate: true });
                }
                $.ajax({
                    type: "POST",
                    url: window.location.origin + '/Report/RevenueReport',
                    data: JSON.stringify(input),
                    datatype: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (res) {
                        $("#ReportFrame").attr("src", 'reports/viewreport.aspx');
                        $('#ReportFrame').load(function () {
                            console.log('iframe loaded');
                            App.stopPageLoading({ animate: true });
                        });
                    }
                })
            }

            vm.init = function () {                
                vm.getCustomers();               
            }

            vm.init();         

        }
    ]);
})();