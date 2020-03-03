import { Component } from '@angular/core';
import { UploadImagesService } from 'src/app/services/upload-images.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  fileData: File[];
  message: string = "Choose Image(s)";

  constructor(private _uploadImagesService: UploadImagesService) { }

  uploadImages(images: any) {
    if(images.length === 0) {
      return;
    }

    this.fileData = images.target.files;

    const formData = new FormData();

    for (let i = 0; i < this.fileData.length; i++) {
      formData.append(this.fileData[i].name, this.fileData[i]);
    }

    this._uploadImagesService.uploadImages(formData).subscribe(res => {
      this.message = "Image(s) Uploaded Successfully";
      console.log(res);
    }, err => {
      this.message = "Error Uploading Image(s)";
      console.log(err);
    });
  }
}
