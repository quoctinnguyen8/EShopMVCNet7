document.addEventListener("alpine:init", () => {
	Alpine.data("category", () => ({

		_list: [],
		_modal: {},
		_noti: {},
		_modalSetting: {
			title: "",
			url: "",
			primaryButtonText: ""
		},
		_updinData: {
			id: 0,
			name: "",
			slug: ""
		},

		init() {
			var config = {
				durations: {
					success: 2000
				},
				labels: {
					success: "Thành công"
				}
			};
			this._modal = new bootstrap.Modal("#categoryUpdinModal");
			this._noti = new AWN(config);

			this.$watch('_updinData.name', (newVal, oldVal) => {
				this._updinData.slug = newVal.toLowerCase()
					.normalize("NFD")
					.replace(/[\u0300-\u036f]/g, "")
					.replace(/đ/g, "d")
					.replace(/[^a-z]/g, "-");
			});

			this.refreshData();
		},
		refreshData() {
			fetch("/Admin/Category/ListAll")
				.then(x => x.json())
				.then(json => {
					this._list = json;
				})
				.catch(err => {
					console.log(err);
				});
		},
		openModalAdd() {
			this._modal.show();
			this._modalSetting = {
				title: "Thêm danh mục sản phẩm",
				url: "/Admin/Category/UpSert",
				primaryButtonText: "Thêm mới"
			};
			// Xóa dữ liệu khi mở modal add
			this._updinData = {
				id: 0,
				name: "",
				slug: ""
			};
		},
		openModalUpdate(id) {
			this._modal.show();
			this._modalSetting = {
				title: "Cập nhật danh mục sản phẩm",
				url: "/Admin/Category/UpSert/" + id,
				primaryButtonText: "Cập nhật"
			}

			// Lấy dữ liệu cho thao tác update
			fetch("/Admin/Category/Detail/" + id)
				.then(res => res.json())
				.then(json => {
					this._updinData = json
				});
		},
		saveCategory() {
			fetch(this._modalSetting.url, {
				method: "POST",
				headers: {
					"Content-Type": "application/json"
				},
				body: JSON.stringify(this._updinData)
			})
				.then(res => {
					this._modal.hide();
					return res.text();
				})
				.then(text => {
					this._noti.success(text);
					this.refreshData();
				})
				.catch(err => {
					alert("Lỗi rồi!");
				})
		},
		removeCategory(id) {
			var url = "/Admin/Category/Delete/" + id;

			this._noti.confirm("Chắc chưa", () => {
				fetch(url)
					.then(res => {
						if (res.status == 200) {
							this._noti.success("Xóa thành công!");
						} else {
							this._noti.alert("Lỗi rồi, không xóa được.");
						}
					});
				this.refreshData();
			});
		}
	}));
});
