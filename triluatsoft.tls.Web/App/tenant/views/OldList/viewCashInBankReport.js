    (function () {
        appModule.controller('tenant.views.OldList.viewCashInBankReport', [
            '$scope',
            '$uibModal',
            function (
                $scope,
                $uibModal

            ) {
                var vm = this;
                vm.page = 1;
                let fromDate = '';
                let toDate = '';
                let reportName = 'BankDepositsReport_';
                vm.dateRangeOptions = app.createDateRangePickerOptions();
                vm.dateRangeModel = {};

                vm.printReport = function (event, printType) {
                    fromDate = vm.dateRangeModel.startDate.format('MM-DD-YYYY');
                    toDate = vm.dateRangeModel.endDate.format('MM-DD-YYYY');

                    event.preventDefault();
                    if (printType == 'inline') {
                        App.startPageLoading({ animate: true });
                    }
                    //var params = '';
                    //load the report by iframe
                    //debugger
                    $("#ReportFrame").attr("src", 'report/CashReport?&txtFromDate1=' + fromDate + "&txtToDate1=" + toDate + "&method=B" + "&reportname=" + reportName + "&lang=" + vm.lang + "&SaveName=Testing Report&ReportType=" + printType);

                    $('#ReportFrame').load(function () {
                        console.log('iframe loaded');
                        App.stopPageLoading({ animate: true });
                    });
                }




            }
        ]);
    })();