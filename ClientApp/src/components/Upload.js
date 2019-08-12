import React from 'react'
import { post } from 'axios';
import { MDBInput } from 'mdbreact';

export class UploadForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            id: 1,
            email: '',
            file: null
        };
        this.handleChange = this.handleChange.bind(this);
        this.submit = this.submit.bind(this);
        this.setFile = this.setFile.bind(this);


    }

    async submit(e) {
        e.preventDefault();

        const url = `api/${this.state.id}`;
        const formData = new FormData();
        formData.append('body', this.state.file);
        formData.append('email', this.state.email);

        const config = {
            headers: {
                'content-type': 'multipart/form-data',
            },
        };
        return post(url, formData, config);
    }

    setFile(e) {
        this.setState({ file: e.target.files[0]});
    }

    handleChange(e) {
        this.setState({ email: e.target.value });
    }

    render() {
        return (
            <div>
            <form onSubmit={e => this.submit(e)}>
                <h1>File Upload</h1>
                    <input type="file" onChange={e => this.setFile(e)} />
                    <br />
                    <MDBInput label="Email" outline icon="envelope" onChange={this.handleChange.bind(this)} required />
                    <h1>{this.state.email}</h1>
                <button type="submit">Upload</button>
            </form>
            </div>
                );
    }
}