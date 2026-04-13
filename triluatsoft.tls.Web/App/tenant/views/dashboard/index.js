(function () {
    appModule.controller('tenant.views.dashboard.index', [
        '$scope',
        '$uibModal',
        'abp.services.app.timeSheet',
        'abp.services.app.dashBoard',
        'abp.services.app.claim',
        'abp.services.app.task',        
        function (
            $scope,
            $uibModal,
            timesheetService,
            dashboardService,
            claimService,
            taskService            
        ) {
            var vm = this;
            vm.myClaimsList = [];
            vm.claim_filter = 0;

            vm.bdrviewby = 10;
            vm.bdrtotalItems = 0;
            vm.bdrcurrentPage = 1;
            vm.bdritemsPerPage = vm.bdrviewby;
            vm.bdrmaxSize = 10; //Number of pager buttons to show

            vm.apprviewby = 10;
            vm.apprtotalItems = 0;
            vm.apprcurrentPage = 1;
            vm.appritemsPerPage = vm.apprviewby;
            vm.apprmaxSize = 10; //Number of pager buttons to show

            vm.subviewby = 10;
            vm.subtotalItems = 0;
            vm.subcurrentPage = 1;
            vm.subitemsPerPage = vm.apprviewby;
            vm.submaxSize = 10; //Number of pager buttons to show

            vm.viewby = 10;
            vm.totalItems = 0;
            vm.currentPage = 1;
            vm.itemsPerPage = vm.viewby;
            vm.maxSize = 10; //Number of pager buttons to show

            vm.pageChanged = function () {
                console.log('Page changed to: ' + vm.currentPage);
            };            

            vm.borderaux = function () {
                dashboardService.loadNotifications()
                .then(function (result) {
                    vm.borderauxList = result.data;
                    vm.bdrtotalItems = vm.borderauxList.length;
                    console.log('borderauxList', vm.borderauxList);
                });
            }

            vm.timesheet = function () {
                timesheetService.getListApproved()
                    .then(function (result) {
                        vm.approvedTimesheet = result.data;
                        vm.apprtotalItems = vm.approvedTimesheet.length;
                        console.log('vm.approvedTimesheet', vm.approvedTimesheet);
                    });
            }

            vm.submission = function () {
                timesheetService.getListSubmission()
                    .then(function (result) {
                        vm.submissionTimesheet = result.data;
                        vm.subtotalItems = vm.submissionTimesheet.length;
                        console.log('vm.submissionTimesheet', vm.submissionTimesheet);
                    });
            }

            vm.outstanding = function () {
                claimService.getTotalOutStanding()
                .then(function (result) {
                    vm.outstandingList = result.data;
                    console.log('vm.outstandingList', vm.outstandingList);
                });
            }
            vm.deadlines = function () {
                taskService.getToDashboard()
                .then(function (result) {
                    vm.deadlinesList = result.data;
                    console.log('vm.deadlinesList', vm.deadlinesList);
                })
            }
            vm.myclaims = function () {
                claimService.getClaimsByEmployee(vm.claim_filter)
                .then(function (result) {
                    vm.myClaimsList = result.data;
                    vm.totalItems = vm.myClaimsList.length;
                    console.log('vm.myClaimsList', vm.myClaimsList);
                })
            }

            //modal claimFolderFile
            vm.openModalClaimFolder = function () {
                debugger
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalClaimFolders.cshtml',
                    controller: 'tenant.views.OldList.modalClaimFolders as vm',
                    backdrop: 'static',
                });

                modalInstance.result.then(function (result) {
                    refeshDatatable();
                });
            }
            vm.borderaux();
            vm.timesheet();
            vm.submission();
            vm.outstanding();
            vm.deadlines();
            vm.myclaims();
            vm.totalItems = vm.myClaimsList.length;
        }
    ]);
})();