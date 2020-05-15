import React, {Component } from 'react';
// import './App.scss';
import { Navbar, Nav, NavDropdown, Form, FormControl, Button, Table } from 'react-bootstrap';
export class bootstrapTest1 extends Component {
    render() {
        return (
            <div>
                OK
                <header>
                    <Form inline>
                        <FormControl type="text" placeholder="Search" className="mr-sm-2" />
                        <Button variant="outline-success">Search</Button>
                    </Form>

                        <Navbar expand="lg" variant="dark" bg="dark">
                            <Navbar.Brand href="#home">React-Bootstrap</Navbar.Brand>
                            <Navbar.Toggle aria-controls="basic-navbar-nav" />
                        </Navbar>

                </header>
                <div className="container">
                    <div className="row mt-5">
                        <div className="col-lg-4 mb-4 grid-margin">
                            <div className="card h-100">
                                <h4 className="card-header">Card Title</h4>
                                <div className="card-body">
                                    <p className="card-text">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente esse necessitatibus neque.</p>
                                </div>
                                <div className="card-footer">
                                    <Button variant="btn btn-primary">Learn More</Button>
                                </div>
                            </div>
                        </div>
                        <div className="col-lg-4 mb-4 grid-margin">
                            <div className="card h-100">
                                <h4 className="card-header">Card Title</h4>
                                <div className="card-body">
                                    <p className="card-text">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Reiciendis ipsam eos, nam perspiciatis natus commodi similique totam consectetur praesentium molestiae atque exercitationem ut consequuntur, sed eveniet, magni nostrum sint fuga.</p>
                                </div>
                                <div className="card-footer">
                                    <Button variant="btn btn-primary">Learn More</Button>
                                </div>
                            </div>
                        </div>
                        <div className="col-lg-4 mb-4 grid-margin">
                            <div className="card h-100">
                                <h4 className="card-header">Card Title</h4>
                                <div className="card-body">
                                    <p className="card-text">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente esse necessitatibus neque.</p>
                                </div>
                                <div className="card-footer">
                                    <Button variant="btn btn-primary">Learn More</Button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="row mb-4">
                        <div className="col-sm-12 grid-margin">
                            <div className="card h-100">
                                <h4 className="card-header">Table</h4>
                                <div className="card-body">
                                    <Table striped bordered hover>
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>First Name</th>
                                                <th>Last Name</th>
                                                <th>Username</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>1</td>
                                                <td>Mark</td>
                                                <td>Otto</td>
                                                <td>@mdo</td>
                                            </tr>
                                            <tr>
                                                <td>2</td>
                                                <td>Jacob</td>
                                                <td>Thornton</td>
                                                <td>@fat</td>
                                            </tr>
                                            <tr>
                                                <td>3</td>
                                                <td colSpan="2">Larry the Bird</td>
                                                <td>@twitter</td>
                                            </tr>
                                        </tbody>
                                    </Table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    renderZZ() {
        return (
            <div >
            <header>
                <Navbar expand="lg" variant="dark" bg="dark">
                    <Navbar.Brand href="#home">React-Bootstrap</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="mr-auto">
                            <Nav.Link href="#home">Home</Nav.Link>
                            <Nav.Link href="#link">Link</Nav.Link>
                            <NavDropdown title="Dropdown" id="basic-nav-dropdown">
                                <NavDropdown.Item>Action</NavDropdown.Item>
                                <NavDropdown.Item>Another action</NavDropdown.Item>
                                <NavDropdown.Item>Something</NavDropdown.Item>
                                <NavDropdown.Divider />
                                <NavDropdown.Item href="#action/3.4">Separated link</NavDropdown.Item>
                            </NavDropdown>
                        </Nav>
                        <Form inline>
                            <FormControl type="text" placeholder="Search" className="mr-sm-2" />
                            <Button variant="outline-success">Search</Button>
                        </Form>
                    </Navbar.Collapse>
                </Navbar>
            </header>
        </div>
        )
    }

}