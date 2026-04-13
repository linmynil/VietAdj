(function () {
    appModule.controller('tenant.views.OldList.ViewLibraryUpload', [
        '$scope',
        '$uibModal',
        'FileUploader',
        'abp.services.app.claimFolderService',
        function (
            $scope,
            $uibModal,
            fileUploader,
            claimFolderService
            ) {
            var vm = this;
            vm.page = 1;
            vm.searchdata = {};
            vm.searchdata.category = 'E';
            vm.searchdata.content = '';

            vm.uploadedFileName = null;

            vm.viewby = 50;
            vm.totalItems = 0;
            vm.currentPage = 1;
            vm.itemsPerPage = vm.viewby;
            vm.maxSize = 10; //Number of pager buttons to show

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };

            vm.fileContractUploader = new fileUploader({
                url: abp.appPath + 'File/UploadFile',
                headers: {
                    "X-XSRF-TOKEN": abp.security.antiForgery.getToken()
                },
                queueLimit: 1,
                autoUpload: true,
                removeAfterUpload: true,
            });

            vm.fileContractUploader.onSuccessItem = function (fileItem, response, status, headers) {
                if (response.success) {
                    vm.uploadedFileName = response.result.fileName;
                    //vm.submitting = false;
                } else {
                    abp.message.error(response.error.message);
                }
            };

            vm.getAll = function () {
                console.log('Load all file upload');
                claimFolderService.getUploadFolderList()
                .then(function (result) {
                    vm.fileList = result.data;
                    vm.totalItems = vm.fileList.length;
                    console.log('File upload list: ', vm.fileList);
                });
            };

            vm.searchLogs = function (event) {
                event.preventDefault();
                console.log('Search file upload: ', vm.searchdata);
                claimFolderService.searchUploadFolderList(vm.searchdata)
                .then(function (result) {
                    vm.fileList = result.data;
                    vm.totalItems = vm.fileList.length;
                    console.log('File upload search list: ', vm.fileList);
                });
            };

            vm.init = function () {
                vm.getAll();
            };

            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            };

            //Message Delete
            vm.deleteUpload = function () {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            //...delete user
                        }
                    });
            };

            //Download file
            vm.download = function (fid, event) {
                event.preventDefault();
                App.startPageLoading({ animate: true });
                console.log('Download: ', fid);

                claimFolderService.downloadLabFile(fid)
                    .then(function (result) {
                        console.log('Download result', result.data);
                        app.downloadTempFile(result.data);
                    })
                    .finally(function () {
                        App.stopPageLoading({ animate: true });
                    });
            };

        }
    ]);
})();