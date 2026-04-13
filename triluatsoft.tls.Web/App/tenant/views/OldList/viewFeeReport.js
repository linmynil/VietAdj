(function () {
    appModule.controller('tenant.views.OldList.viewFeeReport', [
        '$scope',
        '$uibModal', '$window',
        'abp.services.app.timeSheet',
        'abp.services.app.report',
        function (
            $scope,
            $uibModal
            , $window
            , timesheetService
            , reportService
        ) {
            var vm = this;
            vm.page = 0;

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
                $('#from').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#from').on('apply.daterangepicker', function (ev, picker) {
                    $('#from').val(picker.startDate.format('DD-MM-YYYY'));
                });


                $('#to').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#to').on('apply.daterangepicker', function (ev, picker) {
                    $('#to').val(picker.startDate.format('DD-MM-YYYY'));
                });
            });

            vm.init = function () {
                vm.getAll();
            }


            vm.getAll = function () {

                let from = $('#from').val() !== '' ? moment($('#from').val(), "DD-MM-YYYY").format() : undefined;
                let to = $('#to').val() !== '' ? moment($('#to').val(), "DD-MM-YYYY").format() : undefined;
                let page = Math.max(vm.page - 1, 0);//because of server index page from zero

                console.log('getAll', $.extend({}, vm.searchdata, { startDate: from }, { endDate: to }, { page: page }));
                vm.loading = true;
                timesheetService.searchTimeSheet2($.extend({}, vm.searchdata, { startDate: from }, { endDate: to }, { page: page }))
                    .then(function (result) {
                        vm.list = result.data.items;
                        console.log('list', vm.list);
                        vm.totalCount = result.data.totalCount;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });

            };

            vm.pageChanged = function () {
                vm.getAll();
            };

            vm.searchLogs = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e)
                } finally {
                    event.preventDefault();
                }
            }

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 0;
            };

            vm.viewReport = function (timesheetID, empID, lang) {
                console.log('viewReport', timesheetID, empID, lang);
                if (empID === undefined) {                    
                    abp.message.error('Employee is required.');
                    return;
                }

                var url = 'report/timesheetFeeReport?tsID=' + timesheetID + '&empID=' + empID + '&lang=' + lang;
                $window.open(url, "popup", "width=850,height=700,left=100,top=100");
            };

            vm.exportExcel = function (event, timesheetID, lang) {
                event.preventDefault();
                console.log('exportExcel', timesheetID, lang);                

                App.startPageLoading({ animate: true });

                reportService.exportTimesheetFeeReport(timesheetID, lang)
                    .then(function (result) {
                        console.log('exportExcel result', result.data);
                        app.downloadTempFile(result.data);
                    })
                    .finally(function () {
                        App.stopPageLoading({ animate: true });
                    });
            }

            vm.init();

        }
    ]);
})();