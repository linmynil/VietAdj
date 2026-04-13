(function () {
    appModule.controller('tenant.views.OldList.viewWIPReport', [
        '$scope',
        '$uibModal',
        'abp.services.app.customer',
        'abp.services.app.claim',
        function (
            $scope,
            $uibModal,
            customerService,
            claimService
        ) {
            var vm = this;
            let reportName = 'BankDepositsReport_';
            let input = {};
            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeOptions.minDate = new Date(2000, 1, 1);
            vm.dateRangeOptions.maxDate = new Date();
            vm.dateRangeModel = {};

            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
            };

            vm.getOpenClaims = function () {
                claimService.getOpenClaim({})
                    .then(function (result) {
                        console.log('open claims', result.data);
                        vm.item.claims = result.data;
                    });
            };

            vm.getPostData = function () {
                input.txtFromDate = (vm.dateRangeModel.startDate == null) ? null : moment(vm.dateRangeModel.startDate).format('MM-DD-YYYY');
                input.txtToDate = (vm.dateRangeModel.endDate == null) ? null : moment(vm.dateRangeModel.endDate).format('MM-DD-YYYY');
                input.txtClaimID = (vm.ClaimID === undefined) ? "" : vm.ClaimID;
                input.insurer = (vm.insurer === undefined) ? "" : vm.insurer;
                input.rdolisIssued = vm.rdolisIssued;                
                input.lang = vm.lang;
                console.log('Post data: ', input);
                return input;

            };

            vm.printReport = function (event, printType) {
                event.preventDefault();
                vm.getPostData();
                if (printType === 'inline') {
                    App.startPageLoading({ animate: true });
                }
                $.ajax({
                    type: "POST",
                    url: window.location.origin + '/Report/WIPReport',
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
            };

            vm.init = function () {
                vm.getCustomers();
                vm.getOpenClaims();
            }

            vm.init();
        }
    ]);
})();