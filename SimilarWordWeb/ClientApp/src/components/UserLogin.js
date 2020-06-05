import React, { Fragment, useState } from 'react';
import { Form, FormControl, FormGroup, ControlLabel, Col, Button, Label } from 'react-bootstrap';

const UserLogin = ({ name, location,history }) => {
    // Declare a new state variable, which we'll call "count"
    const [word, setWord] = useState({ Username: '', Password: '' });
    const [result, setResult] = useState('');

    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("UserLogin CallUpdateApi");
        console.log(word);
        e.preventDefault();
        fetch('api/users/authenticate', {
            method: 'POST',
            body: JSON.stringify(word),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(user => {
                console.log(user);
                localStorage.setItem('SimilarWordUser', JSON.stringify(user));
                history.push('/');
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
        word[e.target.name] = e.target.value;
        setWord(word);
        setResult('');
    }

    console.log(name);
    return (
        <Fragment>
            <Form horizontal onSubmit={this.CallUpdateApi} >
                <h3 >Login</h3>
                <FormField type='text' label="User Name" name="Username" onChangeHandle={handleChange} val={word.Username} ></FormField>
                <FormField type='text' label="Password" name="Password" onChangeHandle={handleChange} val={word.Password} ></FormField>

                <label>{result}</label>
                <FormGroup>
                    <Col smOffset={2} sm={8}>
                        <button className="primary" onClick={CallUpdateApi}>Login</button>
                    </Col>
                </FormGroup>
            </Form>


        </Fragment>
    );


}


export default UserLogin;
