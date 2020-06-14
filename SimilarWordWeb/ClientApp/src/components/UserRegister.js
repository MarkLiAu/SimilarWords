import React, { Fragment, useState } from 'react';
import { Form, FormControl, FormGroup, ControlLabel, Col, Button, Label } from 'react-bootstrap';
import { resetWarningCache } from 'prop-types';

const UserRegister = ({ name, location, history }) => {
    // Declare a new state variable, which we'll call "count"
    const [inputFields, SetInputFields] = useState({ Email: '', Password: '',FirstName:'',LastName:'' });
    const [result, setResult] = useState('');

    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("UserLogin CallUpdateApi");
        console.log(inputFields);
        e.preventDefault();
        fetch('api/users/register', {
            method: 'POST',
            body: JSON.stringify(inputFields),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.text())
            .then(resp => {
                console.log(resp);
                setResult(resp);
                //localStorage.setItem('SimilarWordUser', JSON.stringify(user));
            })
    }


    const FormField = ({ type, label, name, val, onChangeHandle }) => {
        return (
            <FormGroup controlId={"form" + label}>
                <Col componentClass={ControlLabel} sm={2}>
                    {label}
                </Col>
                <Col sm={8}>
                    {type === 'text' ?
                        <FormControl type={type} name={name} onChange={onChangeHandle} defaultValue={val} placeholder="Enter here" />
                        : <FormControl componentClass={type} name={name} onChange={onChangeHandle} defaultValue={val} placeholder="Enter here" />
                    }
                </Col>
            </FormGroup>
        );
    }

    const handleChange = (e) => {
        console.log(e.target);
        console.log(e.target.name);
        console.log(e.target.value);
        inputFields[e.target.name] = e.target.value;
        SetInputFields(inputFields);
        setResult('');
    }

    console.log(name);
    return (
        <Fragment>
            <Form horizontal onSubmit={this.CallUpdateApi} >
                <h3 >Login</h3>
                <FormField type='text' label="First Name" name="firstName" onChangeHandle={handleChange} val={inputFields.FirstName} ></FormField>
                <FormField type='text' label="Last Name" name="lastName" onChangeHandle={handleChange} val={inputFields.LastName} ></FormField>
                <FormField type='text' label="Email" name="Email" onChangeHandle={handleChange} val={inputFields.Email} ></FormField>
                <FormField type='text' label="Password" name="Password" onChangeHandle={handleChange} val={inputFields.Password} ></FormField>

                <label>{result}</label>
                <FormGroup>
                    <Col smOffset={2} sm={8}>
                        <button className="primary" onClick={CallUpdateApi}>Creat</button>
                    </Col>
                </FormGroup>
            </Form>


        </Fragment>
    );


}


export default UserRegister;
