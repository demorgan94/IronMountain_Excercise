import { Component } from '@angular/core';
import { UploadImagesService } from 'src/app/services/upload-images.service';
import { Image } from "src/app/models/image.model";

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
})
export class HomeComponent {
    fileData: File[];
    txtDownloaded: boolean;
    message: string = "Choose Image(s)";
    messageTxt: string = "Upload .meta File";

    constructor(private _uploadImagesService: UploadImagesService) { }

    UploadImages(images: any) {
        if (images.length === 0) {
            return;
        }

        this.fileData = images.target.files;

        const formData = new FormData();

        for (let i = 0; i < this.fileData.length; i++) {
            formData.append(this.fileData[i].name, this.fileData[i]);
        }

        this._uploadImagesService.uploadImages(formData).subscribe(res => {
            this.message = "Image(s) Uploaded Successfully";
            this.DownloadFile(res);
        }, err => {
            this.message = "Error Uploading Image(s)";
        });
    }

    UploadTxt(txtFile: any) {
        if (txtFile.length === 0) {
            return;
        }

        const formData = new FormData();

        formData.append(txtFile.target.files[0].name, txtFile.target.files[0]);

        this._uploadImagesService.processTxt(formData).subscribe(res => {
            this.messageTxt = "File Readed Successfully";
            this.DownloadFile(res);
        }, err => {
            this.messageTxt = "Error Processing File";
        });
    }

    DownloadFile(res: any) {
        const element = document.createElement('a');
        element.href = URL.createObjectURL(res.file);
        element.download = res.filename;
        document.body.appendChild(element);
        element.click();
    }
}
