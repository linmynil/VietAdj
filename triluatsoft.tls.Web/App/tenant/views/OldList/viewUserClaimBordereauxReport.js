(function () {
    appModule.controller('tenant.views.OldList.viewUserClaimBordereauxReport', [
        '$scope'
        ,'$uibModal'
        , 'abp.services.app.customer',
        'abp.services.app.claim',
        function (
            $scope,
            $uibModal,
            customerService,
            claimService
            ) {
            var vm = this;
            vm.searchdata = {};

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
                //startDate: moment().startOf('day'),
                //endDate: moment().endOf('day')
            };


            vm.exportClaim = function (event) {
                event.preventDefault();
                App.startPageLoading({ animate: true });
                let postdata = vm.getPostData();

                console.log('exportExcel', postdata);

                claimService.exporClaimFunc(postdata)
                    .then(function (result) {
                        console.log('exportExcel result', result.data);
                        app.downloadTempFile(result.data);
                    })
                    .finally(function () {
                        App.stopPageLoading({ animate: true });
                    });
            };

            vm.getPostData = function () {
                let postdata = vm.searchdata;
                postdata.startDate = vm.dateRangeModel.startDate;
                postdata.endDate = vm.dateRangeModel.endDate;
                return postdata;
            };

            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
            };


            vm.printReport = function (event, printType) {
                event.preventDefault();
                console.log(vm.insurer);

                if (printType == 'inline') {
                    App.startPageLoading({ animate: true });
                }
                //var params = '';
                //load the report by iframe
                $("#ReportFrame").attr("src", 'report/claimBorderauxReport?custID=' + vm.insurerID + '&lang=' + vm.lang + "&ReportName=Testing&SaveName=Testing Report&ReportType=" + printType + "&ReportPath=Demo");

                $('#ReportFrame').load(function () {
                    console.log('iframe loaded');
                    App.stopPageLoading({ animate: true });
                });
            }


            vm.init = function () {
                vm.getCustomers();
            }

            vm.init();
        }
    ]);
})();