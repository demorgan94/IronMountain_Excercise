import { Component } from '@angular/core';
import { UploadImagesService } from 'src/app/services/upload-images.service';

@Component({
  selector: 'app-read-zip',
  templateUrl: './read-zip.component.html'
})
export class ReadZipComponent {
  fileData: File;
  message: string = "Choose Zip File";

  constructor(private _uploadImagesService: UploadImagesService) { }

  uploadZip(zip: any) {
    if(zip.length === 0) {
      return;
    }

    this.fileData = zip.target.files[0];

    const formData = new FormData();
    formData.append("zipFile", this.fileData);

    this._uploadImagesService.uploadZip(formData);
  }

}
