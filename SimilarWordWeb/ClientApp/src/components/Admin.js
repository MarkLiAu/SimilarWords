import React, { Fragment, useState } from 'react';
import { Form, FormControl, FormGroup, ControlLabel, Col, Button, Label } from 'react-bootstrap';

const Admin = ({ cmd}) => {
    // Declare a new state variable, which we'll call "count"
    const [result, setResult] = useState('Processing');

    const CallUpdateApi = () => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        fetch('api/Admin/'+cmd, {
            method: 'PUT',
            body: "",
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => { console.log(response); setResult(response); })
    }
    console.log(cmd);

    if (result === 'Processing') CallUpdateApi();

    return (
        <div>
            <h3>Admin</h3>
            {cmd} {result}
        </div>
    );
    

}


export default Admin;
